import json
import pandas as pd
import statsmodels.api as sm
import matplotlib.pyplot as plt
import scipy.stats as st
import os
import shutil

pd.options.display.float_format = "{:,.8f}".format


def findAllFiles(base: str):
    for root, ds, fs in os.walk(base):
        for f in fs:
            yield os.path.join(root, f)


def same_type_result_files(base: str) -> list[str]:
    files = [
        "_".join(".".join(file.split(".")[:-1]).split("_")[:3])
        for file in findAllFile(base)
    ]
    files = set(files)
    return files


def get_same_type_files(file_name: str):
    path = "/".join(file_name.split("/")[:-1])
    file = file_name.split("/")[-1]

    for root, ds, fs in os.walk(path):
        for f in fs:
            if file not in f:
                continue
            yield os.path.join(root, f)


def get_ip(path: str):
    full_path  = os.path.join(path, "metadata.csv")
    # 使用wps打开过后的csv文件header可能需要从9开始读取
    header = pd.read_csv(full_path, header=7, error_bad_lines=False, dtype=str)
    header = header[header["Foo"].isna() == False]
    return header[~header["Tester_Name"].str.contains("Dna")]

def get_slot_ips(path:str):
    full_path = os.path.join(path,"metadata.csv")

    data = pd.read_csv(full_path)

    slot_ips = {}
    for i in range(1,5):
        slot_name = "Slot" + str(i)
        slot_ip = data.loc[i-1,"Slot_Ip"]
        slot_ips[slot_name] = slot_ip
    return slot_ips
 

def read_meta_data(path: str):
    path = os.path.join(path, "metadata.csv")
    return pd.read_csv(path, header=7, dtype=str)


def compare_column(
    a: pd.DataFrame, b: pd.DataFrame, columns: list[str], range: int
) -> bool:
    return all(a[col] == b[col] for col in columns[:range])


def calculate(path: str, meta: pd.DataFrame, result_path: str = ""):
    # check the results path
    if not result_path:
        result_path = os.path.join(path, "results")
    else:
        result_path = os.path.join(result_path, "results")
    if os.path.exists(result_path):
        shutil.rmtree(result_path)
    os.mkdir(result_path)

    ips = get_ip(path)
# 获取所有slot的IP地址
    slot_ips = get_slot_ips(path)
    result_pd = pd.DataFrame(
        columns=[
            "test",
            "Slot_No",
            "IP",
            "Channel_Type",
            "Channel_No",
            "Mode",
            "R_Load",
            "I_Range",
            "Meas_Range",
            "ATE_gain",
            "ATE_offset",
            "Force_gain",
            "Force_offset",
        ]
    )
    for index, row in meta.iterrows():
        file = os.path.join(path, row[-1])
        # 使用wps打开过后的csv文件header可能需要从6开始读取
        data = pd.read_csv(file, header=7)
        x1 = data["ATE Measure"]
        x2 = data["Force Value"]
        y = data["DMM Measure"]
        try:
            # ATE measure - DMM measure
            slope1, intercept1, r_value, p_value, std_err = st.linregress(x1, y)
            # DMM measure - Force Value
            slope2, intercept2, r_value, p_value, std_err = st.linregress(y, x2)
        except:
            slope1, intercept1 = 1, 0
            slope2, intercept2 = 1, 0

        res_row = {
            "test": os.path.basename(file).split(".")[0],
            "Slot_No": row["Slot_Dna"],
            "IP": ips[ips["Tester_Name"] == row["Slot_Dna"]]["Foo"].values[0],
            "Channel_No": row["Channel_No"],
            "Channel_Type": row["Channel_Type"],
            "Mode": row["Mode"],
            "R_Load": row["R_Load"],
            "I_Range": row["I_Range"],
            "Meas_Range": row["Meas_Range"],
            "ATE_gain": slope1,
            "ATE_offset": intercept1,
            "Force_gain": slope2,
            "Force_offset": intercept2,
        }
        result_pd.loc[len(result_pd) + 1] = res_row


    result_pd.to_csv(os.path.join(result_path, "res.csv"))
    slots = result_pd["Slot_No"].unique()

    # split table
    for slot in slots:
        # Force
        Force_table = pd.DataFrame(
            columns=[
                "channel",
                "gain",
                "offset",
                # "range(5V,20V)",
                "range(5uA/20uA/200uA/2mA/60mA/1A)",
                "mode",
            ]
        )

        force_table_form_total = result_pd[result_pd["Slot_No"] == slot][
            result_pd["Mode"].isin(["FVMV", "FIMV"])
        ]
        Force_table["channel"] = force_table_form_total["Channel_No"]
        Force_table["gain"] = force_table_form_total["Force_gain"]
        Force_table["offset"] = force_table_form_total["Force_offset"]
        # 硬件部分认为此处需要有I_Range，但是软件部分还不支持此项
        Force_table["range(5uA/20uA/200uA/2mA/60mA/1A)"] = force_table_form_total[
            "I_Range"
        ]
        Force_table["mode"] = force_table_form_total["Mode"].apply(lambda x: x[:2])
        # Force_table["range(5V,20V)"] = force_table_form_total["Meas_Range"]

      # 获取当前Slot_No对应的IP地址，如果不存在则使用"unknown"
        slot_no = row["Slot_No"]  # 例如 "1", "2", "3", "4"
        slot_name = f"Slot{slot_no}"  # 转换成 "Slot1", "Slot2", "Slot3", "Slot4"
        slot_ip = slot_ips.get(slot_name, "unknown")  # 使用get_slot_ips函数获取IP地址

        # 使用IP地址来构建输出文件名
        Force_table_csv_name = os.path.join(
            result_path,
            f"Dna_{slot_ip}.csv"  # 文件名现在包含IP地址
        )


        with open(Force_table_csv_name, "w+") as csv_file:
            theIp = ips[ips["Tester_Name"] == slot]["Foo"].values[0]
            header = f"dna,{slot}\nsnid,1,\nip,{theIp}\n,,\n,,\n,,\n,,\n,,\n,,\n,,\n"

            csv_file.write(header)
        Force_table.to_csv(Force_table_csv_name, index=False, mode="a+")

        # MV
        MV_table = pd.DataFrame(
            columns=[
                "channel",
                "gain",
                "offset",
                "range",
                # "range(5V,20V)",
                # "range(5uA/20uA/200uA/2mA/60mA/1A)",
            ]
        )
        mv_table_form_total = result_pd[result_pd["Mode"] == "FVMV"][
            result_pd["Slot_No"] == slot
        ]
        MV_table["channel"] = mv_table_form_total["Channel_No"]
        MV_table["gain"] = mv_table_form_total["ATE_gain"]
        MV_table["offset"] = mv_table_form_total["ATE_offset"]
        MV_table["range"] = mv_table_form_total["Meas_Range"] + "_" + mv_table_form_total["I_Range"]
        # 硬件部分认为此处需要有I_Range，但是软件部分还不支持此项
        # MV_table["range(5uA/20uA/200uA/2mA/60mA/1A)"] = mv_table_form_total["I_Range"]
        MV_table.to_csv(
            os.path.join(
                result_path,
                ips[ips["Tester_Name"] == slot]["Foo"].values[0] + "_MV.csv",
            ),
            index=False,
        )

        # 保存其他表格的数据
        MV_table_csv_name = os.path.join(
            result_path,
            f"Dna_{slot_ip}_MV.csv"  # 文件名现在包含IP地址
        )
        MV_table.to_csv(MV_table_csv_name, index=False)
        # MI
        MI_table = pd.DataFrame(
            columns=[
                "channel",
                "gain",
                "offset",
                "range",
            ]
        )
        Mi_table_form_total = result_pd[result_pd["Mode"] == "FVMI"][
            result_pd["Slot_No"] == slot
        ]
        MI_table["channel"] = Mi_table_form_total["Channel_No"]
        MI_table["gain"] = Mi_table_form_total["ATE_gain"]
        MI_table["offset"] = Mi_table_form_total["ATE_offset"]
        MI_table["range"] = Mi_table_form_total["Meas_Range"] + "_" + Mi_table_form_total["I_Range"]
        
        MI_table_csv_name = os.path.join(
            result_path,
            f"Dna_{slot_ip}_MI.csv"  # 文件名现在包含IP地址
        )
        MI_table.to_csv(MI_table_csv_name, index=False)

        
        MI_table.to_csv(
            os.path.join(
                result_path,
                ips[ips["Tester_Name"] == slot]["Foo"].values[0] + "_MI.csv",
            ),
            index=False,
        )


def copy_csv_to_programdata(path: str):
    # 删除C:\ProgramData\Testrong\ATE_Tester\SelfCalibration中含有的csv
    for root, ds, fs in os.walk(
        # 看这里
        os.path.join("C:\ProgramData\Testrong\ATE_Tester\SelfCalibration")
    ):
        for f in fs:
            if ".csv" in f:
                os.remove(os.path.join(root, f))
    # 复制计算后的csv文件到指定位置
    for root, ds, fs in os.walk(path):
        for f in fs:
            if ".csv" in f and "res" not in f:
                shutil.copy(
                    os.path.join(root, f),
                    os.path.join("C:\ProgramData\Testrong\ATE_Tester\SelfCalibration"),
                )


settings = None
with open("./config.json", "r+") as config:
    settings = json.load(config)

calculate(
    settings["meta_path"],
    read_meta_data(settings["meta_path"]),
    settings["result_path"],
)

copy_csv_to_programdata(settings["result_path"])
