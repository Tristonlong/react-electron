/* eslint-disable import/prefer-default-export */

export const getStils = (state: {
  externalCalibration: {
    stils: any[];
  };
}) => state.externalCalibration.stils;

export const getCmd = (state: {
  externalCalibration: {
    cmd: string;
  };
}) => state.externalCalibration.cmd;

export const getConfig = (state: {
  externalCalibration: {
    config: string;
  };
}) => state.externalCalibration.config;

export const getApp = (state: {
  externalCalibration: {
    app: string;
  };
}) => state.externalCalibration.app;

export const getLog = (state: {
  externalCalibration: {
    log: string;
  };
}) => state.externalCalibration.log;

export const getTcps = (state: {
  externalCalibration: {
    tcps: any[];
  };
}) => state.externalCalibration.tcps;

export const getStilPath = (state: {
  externalCalibration: {
    stilPath: string;
  };
}) => state.externalCalibration.stilPath;

export const getPostStilPath = (state: {
  externalCalibration: {
    postStilPathFile: string;
  };
}) => state.externalCalibration.postStilPathFile;

export const getSelectedStilFile = (state: {
  externalCalibration: {
    selectedStilFile: string;
  };
}) => state.externalCalibration.selectedStilFile;

export const getMode = (state: {
  externalCalibration: {
    mode: string;
  };
}) => state.externalCalibration.mode;

export const getResult = (state: {
  externalCalibration: {
    result: any;
  };
}) => state.externalCalibration.result;
