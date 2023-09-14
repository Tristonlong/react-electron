/* eslint-disable react/jsx-boolean-value */
/* eslint-disable dot-notation */
/* eslint-disable prefer-const */
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

import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { useEffect, useState } from 'react';
import Table, { ColumnsType } from 'antd/es/table';
import { Checkbox, Form, Input } from 'antd';
import { AppDispatch } from '../../redux/store';

interface DataType {
  name: string;
  ip: string;
  port: string;
  timeout: string;
  enabled: string;
}

interface props {
  getTcps: () => any;
  setTcps: () => any;
}

const Settings = ({ getTcps, setTcps }: props) => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();

  const tcps = useSelector(getTcps);

  const columns: ColumnsType<DataType> = [
    {
      title: 'Name',
      dataIndex: 'name',
      key: 'name',
      width: 100,
    },
    {
      title: 'IP',
      dataIndex: 'ip',
      key: 'ip',
      render: (value: any, record: DataType, index: number) => {
        return (
          <Form.Item
            validateStatus={
              /^[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}$/g.test(value)
                ? 'success'
                : 'error'
            }
            style={{ margin: 0 }}
            help={
              /^[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}$/g.test(value)
                ? ''
                : 'xxx.xxx.xxx'
            }
          >
            <Input
              value={value}
              onChange={(e) => {
                dispatch(
                  setTcps(
                    tcps.map((value, idx) => {
                      if (idx === index) {
                        let temp = { ...record };
                        temp.ip = e.target.value;
                        return temp;
                      }
                      return value;
                    })
                  )
                );
              }}
            />
          </Form.Item>
        );
      },
    },
    {
      title: 'Port',
      dataIndex: 'port',
      key: 'port',
      width: 120,
      render: (value: any, record: DataType, index: number) => {
        return (
          <Form.Item
            validateStatus={
              /[^0-9]/g.test(value) ||
              Number(value) < 0 ||
              Number(value) > 65535
                ? 'error'
                : 'success'
            }
            style={{ margin: 0 }}
            hasFeedback={true}
            help={
              /[^0-9.]/g.test(value) ||
              Number(value) < 0 ||
              Number(value) > 65535
                ? '0~65535'
                : ''
            }
          >
            <Input
              value={value}
              onChange={(e) => {
                console.log(/[^0-9]/g.test(value));
                dispatch(
                  setTcps(
                    tcps.map((value, idx) => {
                      if (idx === index) {
                        let temp = { ...record };
                        temp.port = e.target.value;
                        return temp;
                      }
                      return value;
                    })
                  )
                );
              }}
            />
          </Form.Item>
        );
      },
    },
    {
      title: 'Timeout',
      dataIndex: 'timeout',
      key: 'timeout',
      width: 120,
      render: (value: any, record: DataType, index: number) => {
        return (
          <Form.Item
            validateStatus={/[^0-9]/g.test(value) ? 'error' : 'success'}
            style={{ margin: 0 }}
            help={/[^0-9.]/g.test(value) ? '仅数字' : ''}
          >
            <Input
              value={value}
              onChange={(e) => {
                console.log(/[^0-9]/g.test(value));
                dispatch(
                  setTcps(
                    tcps.map((value, idx) => {
                      if (idx === index) {
                        let temp = { ...record };
                        temp.timeout = e.target.value;
                        return temp;
                      }
                      return value;
                    })
                  )
                );
              }}
            />
          </Form.Item>
        );
      },
    },
    {
      title: 'Enable',
      dataIndex: 'enabled',
      key: 'enabled',
      width: 120,
      align: 'center',
      render: (value: any, record: DataType, index: number) => {
        return (
          <Checkbox
            checked={value === 'True'}
            onChange={(e) => {
              dispatch(
                setTcps(
                  tcps.map((value, idx) => {
                    if (idx === index) {
                      let temp = { ...record };
                      temp.enabled = e.target.checked ? 'True' : 'False';
                      return temp;
                    }
                    return value;
                  })
                )
              );
            }}
          />
        );
      },
    },
  ];

  // window.electron.ipcRenderer.on('cp-reply', (arg) => {
  //   // eslint-disable-next-line no-console
  //   const msg: string = arg as string;
  //   if (msg.search('All completed') !== -1) {
  //     window.electron.ipcRenderer.sendMessage('kill', cmd);
  //   }
  //   var area = document.getElementById('area');
  //   if (area!.scrollHeight > area!.clientHeight) {
  //     area!.scrollTop = area!.scrollHeight;
  //   }
  // });

  useEffect(() => {
    // console.log(tcps);
  }, [tcps]);

  return (
    <div>
      <Table
        columns={columns}
        dataSource={tcps}
        scroll={{ x: 600 }}
        pagination={{ pageSize: 5 }}
      />
    </div>
  );
};

export default Settings;
