/* eslint-disable import/prefer-default-export */

export const getStils = (state: {
  index: {
    stils: any[];
  };
}) => state.index.stils;

export const getCmd = (state: {
  index: {
    cmd: string;
  };
}) => state.index.cmd;

export const getConfig = (state: {
  index: {
    config: string;
  };
}) => state.index.config;

export const getApp = (state: {
  index: {
    app: string;
  };
}) => state.index.app;

export const getLog = (state: {
  index: {
    log: string;
  };
}) => state.index.log;

export const getTcps = (state: {
  index: {
    tcps: any[];
  };
}) => state.index.tcps;

export const getStilPath = (state: {
  index: {
    stilPath: string;
  };
}) => state.index.stilPath;

export const getSelectedStilFile = (state: {
  index: {
    selectedStilFile: string;
  };
}) => state.index.selectedStilFile;

export const getMode = (state: {
  index: {
    mode: string;
  };
}) => state.index.mode;

export const getResult = (state: {
  index: {
    result: any;
  };
}) => state.index.result;
