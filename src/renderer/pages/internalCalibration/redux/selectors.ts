/* eslint-disable import/prefer-default-export */

export const getStils = (state: {
  internalCalibration: {
    stils: any[];
  };
}) => state.internalCalibration.stils;

export const getCmd = (state: {
  internalCalibration: {
    cmd: string;
  };
}) => state.internalCalibration.cmd;

export const getConfig = (state: {
  internalCalibration: {
    config: string;
  };
}) => state.internalCalibration.config;

export const getApp = (state: {
  internalCalibration: {
    app: string;
  };
}) => state.internalCalibration.app;

export const getLog = (state: {
  internalCalibration: {
    log: string;
  };
}) => state.internalCalibration.log;

export const getTcps = (state: {
  internalCalibration: {
    tcps: any[];
  };
}) => state.internalCalibration.tcps;

export const getStilPath = (state: {
  internalCalibration: {
    stilPath: string;
  };
}) => state.internalCalibration.stilPath;

export const getSelectedStilFile = (state: {
  internalCalibration: {
    selectedStilFile: string;
  };
}) => state.internalCalibration.selectedStilFile;

export const getMode = (state: {
  internalCalibration: {
    mode: string;
  };
}) => state.internalCalibration.mode;

export const getResult = (state: {
  internalCalibration: {
    result: any;
  };
}) => state.internalCalibration.result;
