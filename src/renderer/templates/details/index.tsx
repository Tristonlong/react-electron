/* eslint-disable react/jsx-no-useless-fragment */
/* eslint-disable no-nested-ternary */
/* eslint-disable no-plusplus */
/* eslint-disable prefer-const */
/* eslint-disable @typescript-eslint/no-shadow */
/* eslint-disable no-var */
/* eslint-disable guard-for-in */
/* eslint-disable no-restricted-syntax */
/* eslint-disable no-use-before-define */
/* eslint-disable no-console */
/* eslint-disable no-undef */
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable react/function-component-definition */
import { Button, Card, Input, Modal, Select, Spin, Upload } from 'antd';
import {
  CheckOutlined,
  RedoOutlined,
  SwapRightOutlined,
} from '@ant-design/icons';
import { RcFile } from 'antd/es/upload';
import { useDispatch, useSelector } from 'react-redux';
import {
  setSelectedStilFile,
  setStilPath,
  setStils,
} from 'renderer/pages/index/redux';
import { useEffect, useState } from 'react';
import {
  getConfig,
  getSelectedStilFile,
} from '../../pages/index/redux/selectors';
import Result from '../result';
import './index.css';

const Details = ({
  title,
  loadStatus,
  result,
  msg,
}: {
  title: any;
  loadStatus: boolean;
  result: any;
  msg: string;
}) => {
  const dispatch = useDispatch();

  const toDict = (object: any) => {
    var obj = [];

    for (let x in object.lost_items_failures) {
      obj.push({ key: x, item: x, status: false });
    }
    for (let x in object.ptr_nonlimited_failures) {
      obj.push({ key: x, item: x, status: false });
    }
    for (let x in object.neq_failures) {
      obj.push({ key: x, item: x, status: false });
    }
    return obj;
  };

  useEffect(() => {
    // console.log(result);
  }, [result]);

  return (
    <div>
      <Card
        title={title}
        size="small"
        bodyStyle={
          result
            ? result.status
              ? { backgroundColor: '#ADF0B1' }
              : { backgroundColor: '#F2D3D0' }
            : {}
        }
      >
        <Spin spinning={loadStatus} size="large">
          <div
            style={{
              width: '100%',
            }}
          >
            {result ? (
              result.status ? (
                <div className="success">
                  <CheckOutlined style={{ fontSize: 200 }} />
                  <p style={{ fontSize: 30 }}>ALL PASS</p>
                </div>
              ) : (
                <Result data={result ? toDict(result) : []} />
              )
            ) : (
              <div className="waiting">
                {msg.indexOf('Wait') !== -1 ? (
                  <RedoOutlined style={{ fontSize: 200 }} />
                ) : (
                  <SwapRightOutlined style={{ fontSize: 200 }} />
                )}

                <p style={{ fontSize: 30 }}>{msg}</p>
              </div>
            )}
          </div>
        </Spin>
      </Card>
    </div>
  );
};

export default Details;
