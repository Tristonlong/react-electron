/* eslint-disable no-return-assign */
/* eslint-disable @typescript-eslint/no-shadow */
/* eslint-disable no-console */
/* eslint-disable no-undef */
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable react/function-component-definition */
import { Button, Card, Input, Select, Tooltip, Upload } from 'antd';
import { UploadOutlined, ApiOutlined, FileOutlined } from '@ant-design/icons';
import { RcFile } from 'antd/es/upload';
import { useDispatch, useSelector } from 'react-redux';
import {
  setMode,
  setSelectedStilFile,
  setStilPath,
  setStils,
} from 'renderer/pages/index/redux';
import {
  getConfig,
  getMode,
  getSelectedStilFile,
} from '../../pages/index/redux/selectors';
import './index.css';

const JsonSettingCard = ({
  title,
  value,
  onChange,
  options,
  version,
  stilFolder,
  width,
}: {
  title: any;
  value: any;
  onChange: any;
  options: any;
  version: string;
  stilFolder: string;
  width: number;
}) => {
  const dispatch = useDispatch();

  const configFile = useSelector(getConfig);
  const stilFile = useSelector(getSelectedStilFile);
  const mode = useSelector(getMode);

  const selectPath = (dir: RcFile) => {
    dispatch(setStilPath(dir.path.split('\\').slice(0, -1).join('\\')));
    dispatch(setSelectedStilFile(dir.path));
    window.electron.ipcRenderer.sendMessage('write-config-json', {
      file: configFile,
    });
    return false;
  };

  return (
    <Card title={title} size="small" style={{ width }}>
      <div className="internalSetting">
        <span style={{ margin: 8 }}>ATE Version: </span>
        <Input
          size="small"
          value={version}
          style={{ width: 400, margin: 8 }}
          disabled
        />
      </div>
      <div className="internalSetting">
        <span style={{ margin: 8 }}>STIL Path : </span>
        <Tooltip title={stilFolder}>
          <Upload beforeUpload={selectPath} fileList={[]}>
            <Input
              size="small"
              value={stilFolder}
              style={{ width: 400, margin: 8 }}
            />
          </Upload>
        </Tooltip>
      </div>
      <div className="internalSetting">
        <span style={{ margin: 8 }}>STIL files : </span>
        <Select
          value={value}
          size="small"
          style={{ width: 400, margin: 8 }}
          onChange={onChange}
          options={options}
        />
      </div>
      <div className="internalSetting">
        {/* <span style={{ margin: 8 }}>Pattern files :</span>
        <Select
          size="small"
          style={{ width: 400, margin: 8 }}
        /> */}
      </div>
    </Card>
  );
};

export default JsonSettingCard;
