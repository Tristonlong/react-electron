/* eslint-disable no-else-return */
/* eslint-disable import/order */
/* eslint-disable react-hooks/rules-of-hooks */
/* eslint-disable @typescript-eslint/no-shadow */
/* eslint-disable no-console */
/* eslint-disable no-undef */
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable react/function-component-definition */
import { Button, Card, Input, Select, Upload } from 'antd';
import {
  LoadingOutlined,
  ClockCircleOutlined,
  CheckOutlined,
  CloseOutlined,
} from '@ant-design/icons';
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
import { useEffect, useState } from 'react';

const externalSettingCard = ({
  title,
  onClick,
  connectStatus,
  width,
}: {
  title: any;
  connectStatus: any;
  onClick: any;
  width: number;
}) => {
  const dispatch = useDispatch();

  const configFile = useSelector(getConfig);

  const showStatus = () => {
    if (connectStatus === 1) {
      return <LoadingOutlined style={{ color: '#0099CC' }} />;
    } else if (connectStatus === 0) {
      return <ClockCircleOutlined style={{ color: '#666666' }} />;
    } else if (connectStatus === 2) {
      return <CheckOutlined style={{ color: '#a8c97f' }} />;
    } else {
      return <CloseOutlined style={{ color: '#ba2636' }} />;
    }
  };

  return (
    <Card title={title} size="small" style={{ width }}>
      <div className="deviceStatus">
        <span style={{ margin: 8 }}>Calibration board: </span>
        {showStatus()}
      </div>
      <div className="deviceStatus">
        <span style={{ margin: 8 }}>8 1/2 Digit Multimeter: </span>
        {showStatus()}
      </div>
      <div>
        <Button
          onClick={() => {
            onClick();
          }}
        >
          Check connection
        </Button>
      </div>
    </Card>
  );
};

export default externalSettingCard;
