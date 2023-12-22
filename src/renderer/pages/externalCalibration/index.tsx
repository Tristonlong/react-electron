/* eslint-disable prefer-const */
/* eslint-disable dot-notation */
/* eslint-disable react/jsx-no-duplicate-props */
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

import { useEffect, useState } from 'react';
import {
  Button,
  Input,
  Modal,
  Select,
  Space,
  Upload,
  UploadFile,
  UploadProps,
} from 'antd';
import ExternalSettingCard from 'renderer/templates/externalSettingCard';
import ExternalBoardSettingCard from 'renderer/templates/externalBoardSettingCard';
import ExternalDetails from 'renderer/templates/externalDetails';
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
  getPostStilPath,
} from './redux/selectors';
import {
  setApp,
  setCmd,
  setConfig,
  setLog,
  setMode,
  setPostStilPathFile,
  setResult,
  setSelectedStilFile,
  setStilPath,
  setStils,
  setTcps,
} from './redux';
import Settings from '../settings';
import './index.css';

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
  const postStilPath = useSelector(getPostStilPath);
  const selectedStilFile = useSelector(getSelectedStilFile);
  const mode = useSelector(getMode);
  const result = useSelector(getResult);
  const [connectionStatus, setConnectionStatus] = useState(0);

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [parseResult, setParseResult] = useState(false);
  const [runPostCheck, setRunPostCheck] = useState(false);
  // 增加python打印判断
  const [pythonError, setPythonError] = useState(false);
  const [startLinearRegression, setStartLinearRegression] = useState(false);

  const [loadStatus, setLoadStatus] = useState(false);
  const [showMsg, setShowMsg] = useState('');
  const [version, setVersion] = useState('');

  // pre test
  // 当
  window.electron.ipcRenderer.on('pre-test-reply', (arg) => {
    // eslint-disable-next-line no-console
    const msg: string = arg as string;
    if (msg.search('End') !== -1) {
      let retVal: any = msg.match(/End: [0-9]*/g);
      let retCode: number =
        retVal.length > 0 ? Number(retVal[0].split(' ')[1]) : 1;
      window.electron.ipcRenderer.sendMessage('kill', cmd);
      if (retCode === 0) {
        // 结束后开始执行线性回归
        setStartLinearRegression(true);
        // setLoadStatus(false);
        // setShowMsg('Waiting for post checking');
      }
    } else {
      dispatch(setLog(log + arg));
      var area = document.getElementById('area');
      if (area!.scrollHeight > area!.clientHeight) {
        area!.scrollTop = area!.scrollHeight;
      }
    }
  });

  // linear regression
  window.electron.ipcRenderer.on('linear-regression-reply', (arg: any) => {
    // eslint-disable-next-line no-console
    // 如果Python脚本有输出（意味着出错）,直接终止程序
    if (arg['output']) {
      dispatch(setLog(log + '\nPython输出: ' + arg['output']));
      setPythonError(true);
      window.alert('python错误' + arg['output']);
      // 通过return中止后续代码执行
      return;
    }
    // 执行第二次，带有offset的calibration
    if (arg['ret'] === 0) {
      console.log(66667);
      setStartLinearRegression(false);
      setRunPostCheck(true);
    }
  });

  // post test
  window.electron.ipcRenderer.on('post-test-reply', (arg) => {
    // eslint-disable-next-line no-console
    // set run post flag to false
    setRunPostCheck(false);
    const msg: string = arg as string;
    if (msg.search('End') !== -1) {
      let retVal: any = msg.match(/End: [0-9]*/g);
      let retCode: number =
        retVal.length > 0 ? Number(retVal[0].split(' ')[1]) : 1;
      window.electron.ipcRenderer.sendMessage('kill', cmd);
      if (retCode === 0) {
        // 结束后开始执行结果分析
        setParseResult(true);
      } else {
        dispatch(
          setLog(
            new Date().toLocaleString() +
              log +
              '\nPost test, ATE offscreen program error\n'
          )
        );
      }
      return;
    }
    dispatch(setLog(log + arg));
    var area = document.getElementById('area');
    if (area!.scrollHeight > area!.clientHeight) {
      area!.scrollTop = area!.scrollHeight;
    }
  });

  // 结果解析返回
  window.electron.ipcRenderer.on('parse-post-test-reply', (arg: any) => {
    // eslint-disable-next-line no-console
    setParseResult(false);
    // 执行第二次，带有offset的calibration
    dispatch(
      setLog(
        new Date().toLocaleString() +
          log +
          '\npost-test result parse finished\n'
      )
    );
    if (arg.res.length === 0) {
      dispatch(
        setResult({
          status: true,
        })
      );
      setLoadStatus(false);
      setParseResult(false);
    } else {
      dispatch(
        setResult({
          status: false,
          result: arg.res,
        })
      );
      setLoadStatus(false);
      setParseResult(false);
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
    const postTestStilName =
      configStilPath
        .split('\\')
        [configStilPath.split('\\').length - 1].split('.')[0] + '_post.stil';
    dispatch(setStilPath(stilFolder));
    dispatch(setSelectedStilFile(configStilPath));
    dispatch(setPostStilPathFile(stilFolder + '\\' + postTestStilName));

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

  const handleOk = () => {
    setIsModalOpen(false);
    // neet to check connection status
    setConnectionStatus(2);
  };

  const handleCancel = () => {
    setIsModalOpen(false);
  };

  useEffect(() => {
    window.electron.ipcRenderer.sendMessage('read-version');
    window.electron.ipcRenderer.sendMessage('read-config', {
      cmd: 'type "' + config + '"',
      config: config,
    });
  }, []);

  useEffect(() => {
    if (startLinearRegression) {
      setLoadStatus(true);
      setShowMsg('Running linear regression');
      dispatch(
        setLog(
          log +
            new Date().toLocaleString() +
            '\npre-test finished\nstart to calculate linear regresion:\n'
        )
      );
      window.electron.ipcRenderer.sendMessage('linear-regression', {});
    }
  }, [startLinearRegression]);

  useEffect(() => {
    if (parseResult) {
      setShowMsg('parse the result');
      dispatch(
        setLog(
          new Date().toLocaleString() +
            log +
            '\nATE offscreen program finished\nstart to parse results:\n'
        )
      );
      window.electron.ipcRenderer.sendMessage('parse-post-test', {});
    }
  }, [parseResult]);

  useEffect(() => {
    if (runPostCheck) {
      setShowMsg('Running post-test');
      dispatch(
        setLog(
          new Date().toLocaleString() +
            log +
            '\nlinear regression finished\nstart to post-test:\n'
        )
      );
      window.electron.ipcRenderer.sendMessage('post-test', {
        filePath: postStilPath,
        tcps: tcps,
        cmd: app,
      });
    }
  }, [runPostCheck]);

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
  }, [new Date().toLocaleString() + log]);

  // hook for listenning
  useEffect(() => {
    if (loadStatus === true) {
      // setShowMsg('Running pre-test');
    } else if (showMsg.search('Waiting') !== -1) {
      setShowMsg(showMsg);
    } else if (!result) {
      setShowMsg('Waiting for running');
    } else if (pythonError) {
      setShowMsg('Python file failed');
    } else {
      setShowMsg('');
    }
  }, [result, loadStatus, pythonError]);

  const showModal = () => {
    setIsModalOpen(true);
  };
  const startCalibration = () => {
    dispatch(setResult(null));
    setLoadStatus(true);
    setShowMsg('Running pre-test');
    dispatch(setLog(new Date().toLocaleString() + '\nstart to pre-test:\n'));
    window.electron.ipcRenderer.sendMessage('pre-test', {
      filePath: selectedStilFile,
      tcps: tcps,
      cmd: app,
    });
  };
  const selectedPreStilPath = (value: string) => {
    dispatch(setSelectedStilFile(value));
    if (value.indexOf('DC') !== -1) {
      dispatch(setMode('dc'));
    } else {
      dispatch(setMode('k7'));
    }
    window.electron.ipcRenderer.sendMessage('write-config-json', {
      file: config,
    });
  };

  const selectedPostStilPath = (value: string) => {
    dispatch(setPostStilPathFile(value));
    if (value.indexOf('DC') !== -1) {
      dispatch(setMode('dc'));
    } else {
      dispatch(setMode('k7'));
    }
    window.electron.ipcRenderer.sendMessage('write-config-json', {
      file: config,
    });
  };

  const killCP = () => {
    window.electron.ipcRenderer.sendMessage('kill', cmd);
    dispatch(setResult(null));
    setStartLinearRegression(false);
    setLoadStatus(false);
    dispatch(setLog(null));
    dispatch(setLog(''));
    setRunPostCheck(false);
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
        <div className="controlPannel">
         
          <ExternalSettingCard
            title={'External setting'}
            prePath={selectedStilFile}
            postPath={postStilPath}
            onChangePrePath={selectedPreStilPath}
            onChangePostPath={selectedPostStilPath}
            options={stilFiles}
            version={version}
            stilFolder={stilPath}
            width={550}
          />
          <ExternalBoardSettingCard
            title={'External board setting'}
            onClick={showModal}
            connectStatus={connectionStatus}
            width={400}
          />
        </div>

        <div style={{ margin: 18, textAlign: 'left' }}>
          <Button
            type={'primary'}
            onClick={startCalibration}
            disabled={loadStatus ? true : false}
          >
            External Calibration
          </Button>
          <Button
            style={{ marginLeft: 8 }}
            danger
            type={'primary'}
            onClick={killCP}
          >
            Stop The Self Calibration
          </Button>
        </div>

        <ExternalDetails
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
