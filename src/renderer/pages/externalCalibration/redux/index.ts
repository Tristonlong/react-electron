import { createSlice } from '@reduxjs/toolkit';

export interface IAppIndexState {
  stilPath: string;
  postStilPathFile: string;
  stils: any[];
  selectedStilFile: string;
  cmd: string;
  config: string;
  app: string;
  log: string;
  tcps: any[];
  mode: string;
  result: any;
}

const initialState: IAppIndexState = {
  stilPath: '',
  postStilPathFile: '',
  selectedStilFile: '',
  cmd: '',
  config:
    '.\\SW1.2_55_calibration_RZ\\selfCalibration\\Testrong.OffScreen.Launch.json',
  app: 'C:\\Program Files\\Testrong\\ATE_Tester\\Testrong.OffScreen.exe',
  log: '',
  stils: [],
  tcps: [],
  mode: 'dc',
  result: null,
};

export const { reducer, actions } = createSlice({
  name: 'app/externalCalibration',
  initialState,
  reducers: {
    setStils: (state, action) => {
      state.stils = action.payload;
    },
    setCmd: (state, action) => {
      state.cmd = action.payload;
    },
    setConfig: (state, action) => {
      state.config = action.payload;
    },
    setApp: (state, action) => {
      state.app = action.payload;
    },
    setLog: (state, action) => {
      state.log = action.payload;
    },
    setTcps: (state, action) => {
      state.tcps = action.payload;
    },
    setStilPath: (state, action) => {
      state.stilPath = action.payload;
    },
    setPostStilPathFile: (state, action) => {
      state.postStilPathFile = action.payload;
    },
    setSelectedStilFile: (state, action) => {
      state.selectedStilFile = action.payload;
    },
    setMode: (state, action) => {
      state.mode = action.payload;
    },
    setResult: (state, action) => {
      state.result = action.payload;
    },
  },
});

export const initType = initialState;
export const { setStils } = actions;
export const { setCmd } = actions;
export const { setConfig } = actions;
export const { setApp } = actions;
export const { setLog } = actions;
export const { setTcps } = actions;
export const { setStilPath } = actions;
export const { setSelectedStilFile } = actions;
export const { setMode } = actions;
export const { setResult } = actions;
export const { setPostStilPathFile } = actions;

export default reducer;
