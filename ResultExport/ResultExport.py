import json
import pandas as pd
import numpy as np
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


def read_meta_data(path: str):
    path = os.path.join(path, "metadata.csv")
    # 使用wps打开过后的csv文件header可能需要从10开始读取
    return pd.read_csv(path, header=4)


def get_slots(path: str):
    path = os.path.join(path, "metadata.csv")
    # 使用wps打开过后的csv文件header可能需要从9开始读取
    return pd.read_csv(path, nrows=4, dtype=str)


def compare_column(
    a: pd.DataFrame, b: pd.DataFrame, columns: list[str], range: int
) -> bool:
    return all(a[col] == b[col] for col in columns[:range])

#  calibration 的 x2，y 取mes_range

def get_measure_from_file(file_path, limit=0.0012):
  data = pd.read_csv(file_path, header=10)
  x1 = data["ATE Measure"]
  x2 = data["Force Value"]
  y = data["DMM Measure"]
  meas_range = data["Meas_Range"].astype(float)
  deltas_measure = abs((x1 - y) / meas_range)
  deltas_force = abs((x2 - y) / meas_range)
  for i in range(len(x2)):
    if x2[i] == 0:
      deltas_measure[i] = 0
      deltas_force[i] = 0
  deltas_force[np.isinf(deltas_force)] = 10000
  deltas_measure[np.isinf(deltas_measure)] = 10000
  deltas_force[np.isneginf(deltas_force)] = 10000
  deltas_measure[np.isneginf(deltas_measure)] = 10000
  deltas_measure_values = [delta for delta in deltas_measure.values if delta != 10000]
  validation_force_values = [delta for delta in deltas_force.values if delta != 10000]
  validation_measure = all(delta < limit for delta in deltas_measure_values)
  validation_force = all(delta < limit for delta in validation_force_values)
  return deltas_measure_values, validation_force_values, validation_measure, validation_force
  


def result_export(path: str, meta: pd.DataFrame, result_path: str = ""):
    # check the results path
    if not result_path:
        result_path = os.path.join(path, "results")
    else:
        result_path = os.path.join(result_path, "results")
    if os.path.exists(result_path):
        shutil.rmtree(result_path)
    os.mkdir(result_path)

    # summary
    slots = get_slots(path)
    print("----------------------------------------------------")
    print(slots)
    print("----------------------------------------------------")
    # slots = slots[slots["Foo"].isna() == False]
    summary = pd.DataFrame(
        columns=[
            "Slot_No",
            "Slot_Name",
            "validation",
        ],
        dtype=str,
    )
    for temp_str in slots[slots["Tester_Name"].str.contains("Dna")]["Tester_Name"].values:
        slots = slots.replace(temp_str, temp_str[4])

    # 将 'Tester_Name' 列的值直接用作 'Slot_No' 和 'Slot_Name'
    summary["Slot_No"] = slots["Tester_Name"].str.extract(r'Slot(\d+)')  # 假设名字是 'Slot1', 'Slot2' 等
    summary["Slot_Name"] = slots["Tester_Name"]
    # calibration_state
    calibration_state = pd.DataFrame(
        columns=[
            "test",
            "Slot_No",
            "Channel_Type",
            "Channel_No",
            "Mode",
            "R_Load",
            "I_Range",
            "Meas_Range",
            "calibration_measure",
            "validation_measure",
            "calibration_force",
            "validation_force",
            "calibration_measure_pre",
            "validation_measure_pre",
            "calibration_force_pre",
            "validation_force_pre",
        ]
    )

    metafile = open(os.path.join(path, "metadata.csv"))
    meta_lines = metafile.readlines()
    slot_table = []
    for line in meta_lines:
        if line.find("Slot1_Dna") >= 0:
            slot_table.append([line[line.index(",") + 1:-1], 1])
        elif line.find("Slot2_Dna") >= 0:
            slot_table.append([line[line.index(",") + 1:-1], 2])
        elif line.find("Slot3_Dna") >= 0:
            slot_table.append([line[line.index(",") + 1:-1], 3])
        elif line.find("Slot4_Dna") >= 0:
            slot_table.append([line[line.index(",") + 1:-1], 4])

    for _, row in meta.iterrows():
        print(row)
        file = os.path.join(path, row[-2])
        
        file_bak = os.path.join(path + '_bak', row[-2])
        deltas_measure_values, validation_force_values, validation_measure, validation_force = get_measure_from_file(file)
        deltas_measure_bak, validation_force_bak, measure_bak, force_bak = get_measure_from_file(file_bak, limit=0.005)
        for i in range(len(slot_table)):
            if str(row["Slot_Dna"]) == slot_table[i][0]:
                slot_number = i + 1
        res_row = {
            "test": os.path.basename(file).split(".")[0],
            "Slot_No": str(slot_number),
            "Channel_No": row["Channel_No"],
            "Channel_Type": row["Channel_Type"],
            "Mode": row["Mode"],
            "R_Load": row["R_Load"],
            "I_Range": row["I_Range"],
            "Meas_Range": row["Meas_Range"],
            "calibration_measure": max(deltas_measure_values),
            "validation_measure": validation_measure,
            "calibration_force": max(validation_force_values),
            "validation_force": validation_force,
            "calibration_measure_pre": max(deltas_measure_bak),
            "validation_measure_pre": measure_bak,
            "calibration_force_pre": max(validation_force_bak),
            "validation_force_pre": force_bak,

        }
        calibration_state.loc[len(calibration_state) + 1] = res_row

    # write the state of tests into csv
    calibration_state.to_csv(
        os.path.join(result_path, "calibration_state.csv"), index=False
    )

    # statistics of all slot, summary
    for slot in summary["Slot_No"].values:
        results = calibration_state[calibration_state["Slot_No"] == slot]
        if results.shape[0] > 0:
            slot_validation = all(
                measure == force and measure is True
                for measure, force in zip(
                    results["validation_measure"].values,
                    results["validation_force"].values,
                )
            )
            summary.loc[summary["Slot_No"] == slot, "validation"] = str(slot_validation)
        else:
            summary.loc[summary["Slot_No"] == slot, "validation"] = "N/A"
    summary.to_csv(os.path.join(result_path, "summary.csv"), index=False)

    print("end 123123")


def save_slot_data_to_txt(csv_path, result_path):
    # 读取 calibration_state.csv 文件
    calibration_state_df = pd.read_csv(csv_path)

    # 根据 Slot_No 分组
    grouped = calibration_state_df.groupby('Slot_No')


    # 创建一个空的集合，用于跟踪已经写入文件的组合
    written_combinations = set()

    # 根据 Slot_No 分组
    grouped = calibration_state_df.groupby('Slot_No')
    # 为每个 Slot_No 创建一个文本文件
    for slot_no, group in grouped:
        # 文件名格式：summary_slot(NUMBER).txt
        txt_file_name = f"summary_slot({slot_no}).txt"
        txt_path = os.path.join(result_path, "summary", txt_file_name)
# 获取脚本开始执行的当前时间
        # start_time = datetime.now().strftime("%H:%M:%S %m/%d/%Y")
        with open(txt_path, 'w') as file:
            # 添加头部信息
            file.write("%JOB_START - Beginning Channel_Board_DIB Calibration test on slot {}\n".format(slot_no))
            # file.write("at {}\n".format(start_time))
            file.write("Stil Rev V1.0.0    ATE_Tester Version: 1.2.0\n")
            # 获取对应的 Slot_Name
            
            file.write("DIB # 234124")
            file.write("\n")  # 空行
            file.write("- Starting dib_test\n")
            file.write("- Temperature at PMU is 38.5 deg C\n")
            file.write("- Temperature at DPS is 38.5 deg C\n")
            # 已经写入文件的组合集合
            written_combinations = set()

            # 遍历每一行
            for _, row in group.iterrows():
                combination_key = (row['Channel_Type'], row['I_Range'])
                # 如果这个组合还没写入文件，就写入文件
                if combination_key not in written_combinations:
                    file.write(f"    - Starting {row['Channel_Type']} Measure, {row['I_Range']} Range\n")
                    file.write(f"    - Starting {row['Channel_Type']} Force, {row['I_Range']} Range\n")
                    # 标记此组合为已写入
                    written_combinations.add(combination_key)

            file.write("\n")  # 空行
            # 收集失败的 Channel_No
            # failed_measure_nos = group[group['validation_measure'] == False]['Channel_No'].tolist()
            # failed_force_nos = group[group['validation_force'] == False]['Channel_No'].tolist()
            failed_measure_rows = group[group['validation_measure'] == False]
            failed_force_rows = group[group['validation_force'] == False]   
            # 如果有失败的 Channel_No，将它们合并成一个字符串，并写入文件
            # if failed_measure_nos:
            #     failed_measure_str = ','.join(map(str, failed_measure_nos))
            #     file.write("%FAIL - Slot{} {} channel {} test at Measure Voltage\n".format(
            #         slot_no, group.iloc[0]['Channel_Type'], failed_measure_str))
           # 处理失败的 Measure
            for _, row in failed_measure_rows.iterrows():
                file.write("%FAIL - Slot{} {} channel {} test at Measure Voltage\n".format(
                    slot_no, row['Channel_Type'], row['Channel_No']))
           
           
            file.write("\n")  # 空行
            # 如果有失败的 Force Channel_No，将它们合并成一个字符串，并写入文件
            # if failed_force_nos:
            #     failed_force_str = ','.join(map(str, failed_force_nos))
            #     file.write("%FAIL - Slot{} {} channel {} test at Force Voltage\n".format(
            #         slot_no, group.iloc[0]['Channel_Type'], failed_force_str))
            
# 处理失败的 Force
            for _, row in failed_force_rows.iterrows():
                file.write("%FAIL - Slot{} {} channel {} test at Force Voltage\n".format(
                    slot_no, row['Channel_Type'], row['Channel_No']))           
            
            # 在文件最后添加结束时间
            # end_time = datetime.now().strftime("%H:%M:%S %m/%d/%Y") 
            # file.wirte("%JOB_END - ****FAILED**** \n at {} ".format(end_time))
            # 添加结束的行
            file.write("\n")  # 空行
            file.write("- Writing to System Calibration file - Begin (up to 5 minutes) \n - Writing to System Calibration file - End\n")

if __name__ == '__main__':            
    settings = None
    with open("./config.json", "r+") as config:
        settings = json.load(config)
    result_export(
        settings["meta_path"],
        read_meta_data(settings["meta_path"]),
        settings["result_path"],
    )
    # 确保 summary 文件夹存在
    summary_path = os.path.join(settings["result_path"], "summary")
    if not os.path.exists(summary_path):
        os.makedirs(summary_path)

    # 保存 Slot 数据到文本文件
    calibration_state_csv_path = os.path.join(settings["result_path"], "results", "calibration_state.csv")
    save_slot_data_to_txt(calibration_state_csv_path, settings["result_path"])