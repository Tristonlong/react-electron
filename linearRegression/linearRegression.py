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
        for file in findAllFiles(base)
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
    path = os.path.join(path, "metadata.csv")
    # 使用wps打开过后的csv文件header可能需要从9开始读取
    header = pd.read_csv(path, nrows=8, dtype=str)

    header = header[header["Foo"].isna() == False]
    return header[~header["Tester_Name"]]



def get_ip_new_format(path: str):
    path = os.path.join(path, "metadata.csv")
    header = pd.read_csv(path, nrows=4, dtype=str)
    # 创建一个字典，将'Slot_No'映射到'Slot_Ip'
    ip_dict = {row['Board_Id']: row['Slot_Ip'] for _, row in header.iterrows() if pd.notna(row['Slot_Ip'])}
    return ip_dict


def read_meta_data(path: str):
    path = os.path.join(path, "metadata.csv")
    return pd.read_csv(path, header=6, dtype=str)


def compare_column(
    a: pd.DataFrame, b: pd.DataFrame, columns: list[str], range: int
) -> bool:
    return all(a[col] == b[col] for col in columns[:range])

def fill_missing_rows(df, max_channel=256):
        # 如果DataFrame为空，则从1开始补齐
        if df.empty:
            start_channel = 1
        else:
            # 确保Channel_No列是整数
            df['Channel_No'] = df['Channel_No'].astype(int)
            # 找到现有的最大channel值
            start_channel = df['Channel_No'].max() + 1

        # 生成缺失的channels的数据
        missing_rows = [{'Channel_No': ch, 'gain': 1, 'offset': 0} 
                        for ch in range(start_channel , max_channel + 1)]

        # 如果有缺失的行，则添加到df中
        if missing_rows:
            df = pd.concat([df, pd.DataFrame(missing_rows)], ignore_index=True)

        # 返回按Channel_No排序的DataFrame
        return df.sort_values(by='Channel_No')


def calculate(path: str, meta: pd.DataFrame, result_path: str = ""):
    if not result_path:
        result_path = os.path.join(path, "results")
    else:
        result_path = os.path.join(result_path, "results")
    if os.path.exists(result_path):
        shutil.rmtree(result_path)
    os.mkdir(result_path)
    print(path + '--------------')
    slot_ips = get_ip_new_format(path)
    print(slot_ips)

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
        file = os.path.join(path, row.iloc[-2])

        # 使用wps打开过后的csv文件header可能需要从10开始读取
        data = pd.read_csv(file, header=12)
      
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
        slot = row["Slot_Dna"]  # 获取当前行的Slot_No
        ip = slot_ips.get(slot, "Unknown")  # 从slot_ips字典中获取对应的IP地址
        res_row = {
            "test": os.path.basename(file).split(".")[0],
            "Slot_No": row["Slot_Dna"],
            "IP": ip,
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
        ip = slot_ips.get(slot, "Unknown")
        print(f"Slot: {slot}, IP: {ip}",'-----------------')


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
        Force_table = fill_missing_rows(Force_table)
        # 将 Channel_No 列的值复制到 channel 列
        Force_table['channel'] = Force_table['Channel_No']
        # 检查和调整gain和offset
        force_table_form_total.loc[(force_table_form_total["Force_gain"] < 0.995) | 
                           (force_table_form_total["Force_gain"] > 1.005), 
                           ["Force_gain", "Force_offset"]] = [1, 0]
        Force_table["channel"] = force_table_form_total["Channel_No"]
        Force_table["gain"] = force_table_form_total["Force_gain"]
        Force_table["offset"] = force_table_form_total["Force_offset"]
        # 硬件部分认为此处需要有I_Range，但是软件部分还不支持此项
        Force_table["range(5uA/20uA/200uA/2mA/60mA/1A)"] = force_table_form_total[
            "I_Range"
        ]
        Force_table["mode"] = force_table_form_total["Mode"].apply(lambda x: x[:2])
        # Force_table["range(5V,20V)"] = force_table_form_total["Meas_Range"]
        Force_table_csv_name = os.path.join(
            result_path,
            f"Dna_{ip}.csv",
        )
        Force_table.drop('Channel_No', axis=1, inplace=True)
        with open(Force_table_csv_name, "w+") as csv_file:
            header = f"dna,{slot}\nsnid,1,\nip,{ip}\n,,\n,,\n,,\n,,\n,,\n,,\n,,\n"

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
        MV_table = fill_missing_rows(MV_table)
        mv_table_form_total.loc[(mv_table_form_total["ATE_gain"] < 0.995) | 
                        (mv_table_form_total["ATE_gain"] > 1.005), 
                        ["ATE_gain", "ATE_offset"]] = [1, 0]
        MV_table["channel"] = mv_table_form_total["Channel_No"]
        MV_table["gain"] = mv_table_form_total["ATE_gain"]
        MV_table["offset"] = mv_table_form_total["ATE_offset"]
        MV_table["range"] = mv_table_form_total["Meas_Range"] + "_" + mv_table_form_total["I_Range"]
        # 硬件部分认为此处需要有I_Range，但是软件部分还不支持此项
        # MV_table["range(5uA/20uA/200uA/2mA/60mA/1A)"] = mv_table_form_total["I_Range"]
        
        
        MV_table.to_csv(
            os.path.join(
                result_path,
                # ips[ips["Tester_Name"] == slot]["Foo"].values[0] + "_MV.csv",
            ),
            index=False,
        )

        # MI
        MI_table = pd.DataFrame(
            columns=[
                "channel",
                "gain",
                "offset",
                "range",
                # "range(5V,20V)",
                # "I_Range",
            ]
        )
        Mi_table_form_total = result_pd[result_pd["Mode"] == "FVMI"][
            result_pd["Slot_No"] == slot
        ]
        MI_table = fill_missing_rows(MI_table)
        # 检查和调整gain和offset
        Mi_table_form_total.loc[(Mi_table_form_total["ATE_gain"] < 0.995) | 
                        (Mi_table_form_total["ATE_gain"] > 1.005), 
                        ["ATE_gain", "ATE_offset"]] = [1, 0]
        MI_table["channel"] = Mi_table_form_total["Channel_No"]
        MI_table["gain"] = Mi_table_form_total["ATE_gain"]
        MI_table["offset"] = Mi_table_form_total["ATE_offset"]
        # MI_table["range(5V,20V)"] = Mi_table_form_total["Meas_Range"]
        # MI_table["I_Range"] = Mi_table_form_total["I_Range"]
        MI_table["range"] = Mi_table_form_total["Meas_Range"] + "_" + Mi_table_form_total["I_Range"]
        
        MI_table.to_csv(
            os.path.join(
                result_path,
                # ips[ips["Tester_Name"] == slot]["Foo"].values[0] + "_MI.csv",
            ),
            index=False,
        )


def copy_csv_to_programdata(path: str):
    # 删除C:\ProgramData\Testrong\ATE_Tester\SelfCalibration中含有的csv
    for root, ds, fs in os.walk(
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
