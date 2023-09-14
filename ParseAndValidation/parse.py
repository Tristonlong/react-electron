# -*- coding:utf-8 -*-
# @Source ：
# @Author : lewen.cai
# @Date : 2022/11/15
# @Description :
import json
import logging
import shutil
from prettytable import prettytable

from pystdf.IO import Parser
from pystdf.Writers import XmlWriter
import Stdf_parser
import os
import argparse
import sys

_logger = logging.getLogger(__name__)

if getattr(sys, "frozen", False):
    WORKING_FOLDER = os.path.dirname(os.path.dirname(sys.executable))
else:
    WORKING_FOLDER = os.path.dirname((os.path.dirname(os.path.realpath(__file__))))
XML_EXPECT_FILE = os.path.join(
    WORKING_FOLDER, "ParseAndValidation", "goldenFiles", "DC_golden.xml"
)
XML_EXPECT_DC_FILE = os.path.join(
    WORKING_FOLDER, "ParseAndValidation", "goldenFiles", "DC_golden.xml"
)
XML_EXPECT_K7_FILE = os.path.join(
    WORKING_FOLDER, "ParseAndValidation", "goldenFiles", "K7_golden.xml"
)


parser = argparse.ArgumentParser(description="Parse the results")
parser.add_argument(
    "--file", "-f", help="The stil file needed to be validated", required=True
)
parser.add_argument(
    "--golden",
    "-g",
    help="The golden file for validation",
    default=os.path.join(
        WORKING_FOLDER, "ParseAndValidation", "goldenFiles", "DC_golden"
    ),
)
parser.add_argument(
    "--mode",
    "-m",
    help="The verify mode 'k7'/'dc'",
    default="dc",
)
args = parser.parse_args()

if __name__ == "__main__":
    # make directory of results
    resultPath = os.path.join(WORKING_FOLDER, "ParseAndValidation", "results")

    if not os.path.exists(resultPath):
        os.mkdir(resultPath)
    # parse golden file to .xml if not exist
    if not os.path.exists(f"{args.golden}.xml"):
        print("parse golden ...")
        Stdf_parser.parse(f"{args.golden}.stdf", args.mode)

    stdf = Stdf_parser.parse(args.file, verify_mode=args.mode)
    lost_items_failures, ptr_nonlimited_failures, neq_failures = stdf.get_fail_results()
    with open(os.path.join(resultPath, "result.json"), "w", encoding="utf-8") as file:
        json.dump(
            {
                "lost_items_failures": {
                    str(k): lost_items_failures[k] for k in lost_items_failures
                },
                "ptr_nonlimited_failures": {
                    str(k): ptr_nonlimited_failures[k] for k in ptr_nonlimited_failures
                },
                "neq_failures": {str(k): neq_failures[k] for k in neq_failures},
                "status": stdf.is_passed(),
            },
            file,
        )

    # print(lost_items_failures)
    # print(ptr_nonlimited_failures)
    # print(neq_failures)
    # print(stdf.is_passed())
