# Linear regression for ATE

## Introduction

This is a python script for calculating the scope and bias of pre-testing.

## Configuration

1. Ensure your python env, `pip install -r requirements.txt`
2. There is a configuration file, which is named `config.json`, all settings are included in it.
3. The keys are `meta_path` and `result_path`. The `meta_path` is the origin source data folder path, this folder should contain a `metadata.csv`. The `result_path` is the output of this script, it includes 'gain', 'offset', 'r value', 'p value', and 'std_error'.
4. The template of the config file is:

```
{
    "meta_path": ".\\Data\\",
    "result_path": ".\\"
}
```

## How to use it

1. Modifing the `config.json` file to that you wanted.
2. Run this command in your console:

```
cd to\\this\\project\\path && python linearRegression.py
```
