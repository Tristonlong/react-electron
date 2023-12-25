/* eslint-disable consistent-return */
/* eslint-disable no-unneeded-ternary */
/* eslint-disable import/no-absolute-path */
/* eslint-disable react/no-unstable-nested-components */
/* eslint-disable react/jsx-props-no-spreading */
/* eslint-disable react/jsx-no-undef */
/* eslint-disable vars-on-top */
/* eslint-disable object-shorthand */
/* eslint-disable jsx-a11y/control-has-associated-label */
/* eslint-disable react/button-has-type */
/* eslint-disable react/jsx-curly-brace-presence */
/* eslint-disable default-case */
/* eslint-disable no-undef */
/* eslint-disable no-param-reassign */
/* eslint-disable prefer-template */
/* eslint-disable camelcase */
/* eslint-disable no-console */
/* eslint-disable prefer-destructuring */
/* eslint-disable no-var */
/* eslint-disable react-hooks/exhaustive-deps */
/* eslint-disable @typescript-eslint/no-shadow */
/* eslint-disable react/function-component-definition */
/* eslint-disable @typescript-eslint/no-unused-vars */

// import './index.less';

import PropTypes from 'prop-types';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { useEffect, useState, useRef } from 'react';
import {
  Button,
  Input,
  Modal,
  Progress,
  Select,
  Space,
  Upload,
  UploadFile,
  UploadProps,
} from 'antd';
import JsonSettingCard from 'renderer/templates/jsonSettingCard';
import Details from 'renderer/templates/details';
import { AppDispatch } from '../../redux/store';
import {
  getApp,
  getCmd,
  getStils,
  getConfig,
  getLog,
  getTcps,
  getStilPath,
  getSelectedStilFile,
  getMode,
  getResult,
} from './redux/selectors';
import {
  setApp,
  setCmd,
  setConfig,
  setLog,
  setMode,
  setResult,
  setSelectedStilFile,
  setStilPath,
  setStils,
  setTcps,
} from './redux';
import Settings from '../settings';
import { current } from '@reduxjs/toolkit';

const { TextArea } = Input;

// This function will output the lines from the script
// and will return the full combined output
// as well as exit code when it's done (using the callback).

const Index = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();

  const app = useSelector(getApp);
  const cmd = useSelector(getCmd);
  const stilFiles = useSelector(getStils);
  const config = useSelector(getConfig);
  const log = useSelector(getLog);
  const tcps = useSelector(getTcps);
  const stilPath = useSelector(getStilPath);
  const selectedStilFile = useSelector(getSelectedStilFile);
  const mode = useSelector(getMode);
  const result = useSelector(getResult);

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [parseResult, setParseResult] = useState(false);
  const [loadStatus, setLoadStatus] = useState(false);
  const [showMsg, setShowMsg] = useState('');
  const [version, setVersion] = useState('');
  const [scannedCode, setScannedCode] = useState('');
  const scanBuffer = useRef('');


  window.electron.ipcRenderer.on('cp-reply', (arg) => {
    // eslint-disable-next-line no-console
    const msg: string = arg as string;
    if (msg.search('Exit:') !== -1) {
      window.electron.ipcRenderer.sendMessage('kill', cmd);
      setParseResult(true);
    }
    dispatch(setLog(log + arg));
    var area = document.getElementById('area');
    if (area!.scrollHeight > area!.clientHeight) {
      area!.scrollTop = area!.scrollHeight;
    }
  });

  window.electron.ipcRenderer.on('read-version-reply', (arg) => {
    // eslint-disable-next-line no-console
    setVersion(arg as string);
  });

  window.electron.ipcRenderer.on('read-config-reply', (arg) => {
    // eslint-disable-next-line no-console
    const msg: string = arg as string;
    const configs = JSON.parse(msg).Configurations[0];
    const configStilPath: string = configs.STILPath as string;
    const stilFolder = configStilPath.split('\\').slice(0, -1).join('\\');
    dispatch(setStilPath(stilFolder));
    dispatch(setSelectedStilFile(configStilPath));

    window.electron.ipcRenderer.sendMessage('kill', cmd);
  });

  window.electron.ipcRenderer.on('search-path-reply', (arg) => {
    // eslint-disable-next-line no-console
    const msg: string = arg as string;
    dispatch(
      setStils([
        ...stilFiles,
        ...msg
          .split('\r\n')
          .filter((value: string) => value && value.indexOf('.stil') !== -1)
          .map((value) => {
            return { value: stilPath + '\\' + value, label: value };
          }),
      ])
    );
    window.electron.ipcRenderer.sendMessage('kill', cmd);
    window.electron.ipcRenderer.sendMessage('write-config-json', {
      file: config,
    });
  });

  window.electron.ipcRenderer.on('read-stil-reply', (arg) => {
    // eslint-disable-next-line no-console
    const msg: string = arg as string;
    const block = /TCP \{[^}]*\}/g;
    const content = /TCP[^;{}]*/g;

    dispatch(
      setTcps(
        msg!
          .match(block)![0]
          .match(content)
          ?.filter((value: string) => {
            return /\d/g.test(value);
          })
          ?.map((value: string) => {
            const splited = value.split('\t');
            return {
              key: splited[0],
              name: splited[0],
              ip: splited[1],
              port: splited[2],
              timeout: splited[3],
              enabled: splited[4],
            };
          })
      )
    );
  });

  window.electron.ipcRenderer.on('run-parse-result-reply', (arg) => {
    // eslint-disable-next-line no-console
    var msg = arg as any;
    dispatch(setLog(log + '\nparse result:' + msg.status + '\n'));

    dispatch(setResult(arg));
    setLoadStatus(false);
    setParseResult(false);
  });

  const showModal = () => {
    setIsModalOpen(true);
  };

  const handleOk = () => {
    setIsModalOpen(false);
    setLoadStatus(true);
    window.electron.ipcRenderer.sendMessage('cover-tcp', {
      filePath: selectedStilFile,
      tcps: tcps,
      cmd: app,
    });
  };

  const handleCancel = () => {
    setIsModalOpen(false);
  };

  const selected = (value: string) => {
    dispatch(setSelectedStilFile(value));
    if (value.indexOf('DC') !== -1) {
      dispatch(setMode('dc'));
    } else {
      dispatch(setMode('k7'));
    }
    window.electron.ipcRenderer.sendMessage('write-config-json', {
      file: config,
      stilPath: value,
    });
  };

  useEffect(() => {
    window.electron.ipcRenderer.sendMessage('read-version');
    window.electron.ipcRenderer.sendMessage('read-config', {
      cmd: 'type "' + config + '"',
      config: config,
    });
  }, []);

  useEffect(() => {
    if (parseResult) {
      window.electron.ipcRenderer.sendMessage('run-parse-result', {
        mode,
      });
    }
  }, [parseResult]);

  useEffect(() => {
    dispatch(setStils([]));
    if (stilPath) {
      window.electron.ipcRenderer.sendMessage('search-path', {
        cmd: 'dir /b "' + stilPath + '"',
      });
    }
  }, [stilPath]);

  useEffect(() => {
    if (selectedStilFile) {
      selected(selectedStilFile);
      window.electron.ipcRenderer.sendMessage('read-stil', selectedStilFile);
    }
  }, [selectedStilFile]);

  useEffect(() => {
    if (log) {
      var area = document.getElementById('area');
      if (area!.scrollHeight > area!.clientHeight) {
        area!.scrollTop = area!.scrollHeight;
      }
    }
  }, [log]);

  useEffect(() => {
    if (loadStatus === true) {
      setShowMsg('Running');
    } else if (!result) {
      setShowMsg('Waiting for running');
    } else {
      setShowMsg('');
    }
    console.log('asdfasdf');
  }, [result, loadStatus]);

  useEffect(() => {
    const handleScan = (e: KeyboardEvent) => {
      console.log(e.key);
      setScannedCode((prevScannedCode) => prevScannedCode + e.key);
    };
    window.addEventListener('keydown', handleScan);
    return () => {
      window.removeEventListener('keydown', handleScan);
    };
  }, []);
  useEffect(() => {
    if (scannedCode) {
      window.electron.ipcRenderer.sendMessage('save-scanned-code', scannedCode)
    }
  }, [scannedCode]);
  const openCP = () => {
    dispatch(setResult(null));
    showModal();
  };
  const killCP = () => {
    window.electron.ipcRenderer.sendMessage('kill', cmd);
    dispatch(setResult(null));
    setLoadStatus(false);
    dispatch(setLog(null));
    dispatch(setLog(''));
  };

  return (
    <div style={{ height: window.innerHeight }}>
      <Modal
        title="Basic Modal"
        open={isModalOpen}
        onOk={handleOk}
        onCancel={handleCancel}
        width={700}
      >
        <Settings setTcps={setTcps} getTcps={getTcps} />
      </Modal>
      <div>
        <JsonSettingCard
          title={'Internal setting'}
          value={selectedStilFile}
          onChange={selected}
          options={stilFiles}
          version={version}
          stilFolder={stilPath}
          width={550}
        />
        <div style={{ margin: 18, textAlign: 'left' }}>
          <Button type={'primary'} onClick={openCP}>
            Start Self Calibration
          </Button>
          <Button
            style={{ marginLeft: 8 }}
            danger
            type={'primary'}
            onClick={killCP}
          >
            Stop The Self Calibration
          </Button>
          <div>
            <div>扫描结果：{scannedCode}</div>
          </div>
          <div>
            <div>进度条：</div>
            <Progress percent={50} status="active" />
          </div>
        </div>

        <Details
          title={'Results:'}
          msg={showMsg}
          loadStatus={loadStatus}
          result={result}
        />

        <div style={{ margin: 8 }}>
          <p>log info: </p>
          <TextArea
            id="area"
            showCount
            autoSize={{ minRows: 10, maxRows: 10 }}
            style={{ marginBottom: 24 }}
            value={log}
            placeholder="disable resize"
          />
        </div>
      </div>
    </div>
  );
};

export default Index;
