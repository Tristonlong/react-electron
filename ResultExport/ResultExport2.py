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
    return pd.read_csv(path, header=9)


def get_slots(path: str):
    path = os.path.join(path, "metadata.csv")
    # 使用wps打开过后的csv文件header可能需要从9开始读取
    return pd.read_csv(path, nrows=8, dtype=str)


def compare_column(
    a: pd.DataFrame, b: pd.DataFrame, columns: list[str], range: int
) -> bool:
    return all(a[col] == b[col] for col in columns[:range])


def get_measure_from_file(file_path, limit=0.001):
  data = pd.read_csv(file_path, header=9)

  x1 = data["ATE Measure"]
  x2 = data["Force Value"]
  y = data["DMM Measure"]
  deltas_measure = abs((x1 - y) / y)
  deltas_force = abs((x2 - y) / x2)
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
    slots = slots[slots["Foo"].isna() == False]

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
    summary["Slot_No"] = slots[~(slots["Foo"].str.contains(".", regex=False))]["Tester_Name"]
    summary["Slot_Name"] = slots[~(slots["Foo"].str.contains(".", regex=False))][
        "Foo"
    ]

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
        file = os.path.join(path, row[-1])
        file_bak = os.path.join(path + '_bak', row[-1])
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
            slot_validation = True
            for k in range(len(results["validation_measure"].values)):
                slot_validation = slot_validation and results["validation_measure"].values[k] and results["validation_force"].values[k] #and results["validation_measure_pre"].values[k] and results["validation_force_pre"].values[k]
            summary.loc[summary["Slot_No"] == slot, "validation"] = str(slot_validation)
        else:
            summary.loc[summary["Slot_No"] == slot, "validation"] = "N/A"
    summary.to_csv(os.path.join(result_path, "summary.csv"), index=False)

settings = None
with open("./config.json", "r+") as config:
    settings = json.load(config)
result_export(
    settings["meta_path"],
    read_meta_data(settings["meta_path"]),
    settings["result_path"],
)
