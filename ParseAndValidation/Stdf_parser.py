# -*- coding:utf-8 -*-
# @Source ：
# @Author : lewen.cai
# @Date : 2022/11/15
# @Description :
import logging
import shutil
from prettytable import prettytable

from pystdf.IO import Parser
from pystdf.Writers import XmlWriter
import xml.sax
import sys
import os

_logger = logging.getLogger(__name__)

if getattr(sys, "frozen", False):
    WORKING_FOLDER = os.path.dirname(os.path.dirname(sys.executable))
else:
    WORKING_FOLDER = os.path.dirname((os.path.dirname(os.path.realpath(__file__))))
XML_EXPECT_FILE = os.path.join(
    WORKING_FOLDER, "ParseAndValidation", "goldenFiles", "dc_golden.xml"
)
XML_EXPECT_DC_FILE = os.path.join(
    WORKING_FOLDER, "ParseAndValidation", "goldenFiles", "dc_golden.xml"
)
XML_EXPECT_K7_FILE = os.path.join(
    WORKING_FOLDER, "ParseAndValidation", "goldenFiles", "k7_golden.xml"
)


# SITE_NUM="1" PART_FLG="1"
class StdfHander(xml.sax.handler.ContentHandler):
    def __init__(self, expect_fail_ptr):
        super().__init__()
        self.expect_fail_ptr = expect_fail_ptr
        self.CurrentData = ""
        self.ptr_fail_details = dict()
        self.data_details = dict()
        self.site_num = "0"
        self.sites_time = dict()
        self.__loops_time = list()

    def startElement(self, name, attrs):
        self.CurrentData = name
        if self.CurrentData == "Ptr":
            result = attrs["RESULT"]
            lo_limit = attrs["LO_LIMIT"]
            hi_limit = attrs["HI_LIMIT"]
            head_num = attrs["HEAD_NUM"]
            site_num = attrs["SITE_NUM"]
            key = ("Ptr", head_num, site_num)
            test_txt = attrs["TEST_TXT"]
            if not self.data_details.get(key):
                self.data_details[key] = set()
            self.data_details[key].add(test_txt)
            if result and float(lo_limit) <= float(result) <= float(hi_limit):
                if self.expect_fail_ptr.get(("Ptr", head_num, site_num, test_txt)):
                    self.ptr_fail_details[
                        ("Ptr", head_num, site_num, test_txt)
                    ] = ",".join([lo_limit, result, hi_limit])
            else:
                if not self.expect_fail_ptr.get(("Ptr", head_num, site_num, test_txt)):
                    self.ptr_fail_details[
                        ("Ptr", head_num, site_num, test_txt)
                    ] = ",".join([lo_limit, result, hi_limit])

        if self.CurrentData == "Prr":
            head_num = attrs["HEAD_NUM"]
            site_num = attrs["SITE_NUM"]
            if site_num <= self.site_num:
                self.__loops_time.append(self.sites_time)
                self.sites_time = dict()
            self.sites_time[site_num] = int(attrs["TEST_T"])
            self.site_num = site_num
            part_flg = attrs["PART_FLG"]
            hard_bin = attrs["HARD_BIN"]
            soft_bin = attrs["SOFT_BIN"]
            key = ("Prr", head_num, site_num)
            if not self.data_details.get(key):
                self.data_details[key] = set()
            self.data_details[key].add("_".join([part_flg, hard_bin, soft_bin]))

        if self.CurrentData == "Ftr":
            test_txt = attrs["TEST_TXT"]
            test_flg = attrs["TEST_FLG"]
            rslt_txt = attrs["RSLT_TXT"]
            head_num = attrs["HEAD_NUM"]
            site_num = attrs["SITE_NUM"]
            key = ("Ftr", head_num, site_num, test_txt)
            if not self.data_details.get(key):
                self.data_details[key] = set()
            self.data_details[key].add("_".join([test_flg, rslt_txt]))

        if self.CurrentData == "Hbr":
            hbin_num = attrs["HBIN_NUM"]
            head_num = attrs["HEAD_NUM"]
            site_num = attrs["SITE_NUM"]
            key = ("Hbr", head_num, site_num)
            if not self.data_details.get(key):
                self.data_details[key] = set()
            self.data_details[key].add(hbin_num)

        if self.CurrentData == "Sbr":
            sbin_num = attrs["SBIN_NUM"]
            head_num = attrs["HEAD_NUM"]
            site_num = attrs["SITE_NUM"]
            key = ("Sbr", head_num, site_num)
            if not self.data_details.get(key):
                self.data_details[key] = set()
            self.data_details[key].add(sbin_num)

    def endDocument(self):
        if self.sites_time:
            self.__loops_time.append(self.sites_time)

    def get_results(self):
        return self.data_details, self.ptr_fail_details, self.__loops_time

    def get_loops_time(self):
        return self.__loops_time


class __Stdf2xml_parser(object):
    def __init__(self, path, verify_mode=None):
        self.__loops_time = list()
        self.std_fail_ptr = dict()
        self.stdf2xml = self.__process_file(path)
        if verify_mode == "dc":
            xml_expect = XML_EXPECT_DC_FILE
        elif verify_mode == "k7":
            xml_expect = XML_EXPECT_K7_FILE
        else:
            xml_expect = XML_EXPECT_FILE
        shutil.copyfile(xml_expect, os.path.join(os.path.dirname(path), "selfCal.xml"))
        self.__expect_details, self.expect_fail_ptr, _ = self.__parse_results(
            xml_expect, self.std_fail_ptr
        )
        _logger.info("以下是标准文件里面不在limit范围内的测试项：")
        self.expect_fail_ptr and self.pretty_print_ptr(self.expect_fail_ptr)
        self.std_fail_ptr = self.expect_fail_ptr
        (
            self.__actual_details,
            self.__ptr_nonlimited_failures,
            self.__loops_time,
        ) = self.__parse_results(self.stdf2xml, self.std_fail_ptr)

        self.__lost_items_failures = dict()
        self.__neq_failures = dict()
        self.process_results()

    def __process_file(self, stdf_file):
        with open(stdf_file, "rb") as f:
            p = Parser(inp=f)
            out_file = stdf_file[: stdf_file.rfind(".")] + ".xml"
            with open(out_file, "w") as fout:
                p.addSink(XmlWriter(stream=fout))
                p.parse()
        return out_file

    def __parse_results(self, xml_path, expect_fail_ptr):
        parser = xml.sax.make_parser()
        parser.setFeature(xml.sax.handler.feature_namespaces, 0)
        stdfHander = StdfHander(expect_fail_ptr)
        parser.setContentHandler(stdfHander)
        parser.parse(xml_path)
        return stdfHander.get_results()

    def is_passed(self):
        if (
            self.__lost_items_failures
            or self.__neq_failures
            or self.__ptr_nonlimited_failures
        ):
            _logger.info("tsdata has_exception->1")
            return False
        return True

    def process_results(self):
        for key in self.__expect_details:
            if not self.__actual_details.get(key):
                self.__lost_items_failures[key] = self.__expect_details[key]
            elif self.__actual_details[key] != self.__expect_details[key]:
                lost_items = [
                    i
                    for i in self.__expect_details[key]
                    if i not in self.__actual_details[key]
                ]
                if lost_items:
                    self.__lost_items_failures[key] = lost_items
                neq_items = [
                    i
                    for i in self.__actual_details[key]
                    if i not in self.__expect_details[key]
                ]
                if neq_items:
                    self.__neq_failures[key] = neq_items

    def get_fail_results(self):
        return (
            self.__lost_items_failures,
            self.__ptr_nonlimited_failures,
            self.__neq_failures,
        )

    def get_loops_time(self):
        return self.__loops_time

    def pretty_print_ptr(self, ptr_dict):
        table = prettytable.PrettyTable(
            [
                "test_type",
                "head_num",
                "site_num",
                "test_item",
                "l_limit",
                "value",
                "h_limit",
            ]
        )
        for f_key, f_value in ptr_dict.items():
            print_list = list(f_key)
            print_list.extend(f_value.split(","))
            table.add_row(print_list)
        _logger.info(table)


def parse(path, verify_mode=None):
    return __Stdf2xml_parser(path, verify_mode=verify_mode)


if __name__ == "__main__":
    fin = r"\\172.18.32.10\Public_folder\temp\zxy\SW_1.1.2\data\K7\FlowLog_2023_05_26_16_49_33.799.stdf"
    stdf = parse(fin, verify_mode="k7")
    lost_items_failures, ptr_nonlimited_failures, neq_failures = stdf.get_fail_results()
    print(lost_items_failures)
    print(ptr_nonlimited_failures)
    print(neq_failures)
    print(stdf.is_passed())
