/* eslint-disable dot-notation */
/* eslint-disable no-var */
/* eslint-disable no-bitwise */
/* eslint-disable no-plusplus */
/* eslint-disable vars-on-top */
/* eslint-disable no-param-reassign */
/* eslint-disable react/jsx-no-undef */
/* eslint-disable promise/always-return */
/* eslint-disable promise/catch-or-return */
/* eslint-disable @typescript-eslint/no-shadow */
/* eslint-disable react/no-unused-prop-types */
/* eslint-disable no-use-before-define */
/* eslint-disable no-console */
/* eslint-disable no-undef */
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable react/function-component-definition */
import { Button, Card, Input, Modal, Select, Table, Upload } from 'antd';
import { StopOutlined, ApiOutlined, FileOutlined } from '@ant-design/icons';
import { useDispatch, useSelector } from 'react-redux';
import { ColumnsType } from 'antd/es/table';
// import ReactHTMLTableToExcel from 'react-html-table-to-excel';
import XLSX from 'xlsx/xlsx';
import './index.css';
import { useEffect } from 'react';

interface DataType {
  item: string;
  status: string;
}

const Result = ({ data }: { data: any }) => {
  const dispatch = useDispatch();

  const DEFAULT_COLUMN_WIDTH = 20;

  const columns: ColumnsType<DataType> = [
    {
      title: 'Item',
      dataIndex: 'item',
      key: 'item',
      width: 500,
      align: 'center',
      render: (value: any, record: DataType, index: number) => {
        // console.log('ppppppppp', value);
        return value;
      },
    },
    {
      title: 'Status',
      dataIndex: 'status',
      key: 'status',
      render: (value: any, record: DataType, index: number) => {
        return <StopOutlined style={{ color: 'red' }}>Failed</StopOutlined>;
      },
      align: 'center',
    },
  ];

  // const onExportBasicExcel = (e: any) => {
  //   const cloneDivNode = document.getElementById('antdTable')!.cloneNode(true);
  //   const table = document.createElement('table');
  //   table.appendChild(cloneDivNode);
  //   table.setAttribute('id', 'table-to-xls');
  //   document.getElementById('root')!.appendChild(table);
  //   document.getElementById('test-table-xls-button')!.click();
  //   setTimeout(() => {
  //     document.getElementById('root')!.removeChild(table);
  //   }, 500);
  // };

  const openDownloadDialog = (url: any, saveName: string) => {
    if (typeof url === 'object' && url instanceof Blob) {
      url = URL.createObjectURL(url); // 创建blob地址
    }
    const aLink = document.createElement('a');
    aLink.href = url;
    aLink.download = saveName || ''; // HTML5新增的属性，指定保存文件名，可以不要后缀，注意，file:///模式下不会生效
    let event;
    if (window.MouseEvent) event = new MouseEvent('click');
    else {
      event = document.createEvent('MouseEvents');
      event.initMouseEvent(
        'click',
        true,
        false,
        window,
        0,
        0,
        0,
        0,
        0,
        false,
        false,
        false,
        false,
        0,
        null
      );
    }
    aLink.dispatchEvent(event);
  };

  const sheet2blob = (sheet: any, sheetName: any) => {
    // 将一个sheet转成最终的excel文件的blob对象，然后利用URL.createObjectURL下载
    sheetName = sheetName || 'sheet1';
    // eslint-disable-next-line no-var
    var workbook: any = {
      SheetNames: [sheetName],
      Sheets: {},
    };
    workbook.Sheets[sheetName] = sheet;
    // 生成excel的配置项
    const wopts = {
      bookType: 'xlsx', // 要生成的文件类型
      bookSST: false, // 是否生成Shared String Table，官方解释是，如果开启生成速度会下降，但在低版本IOS设备上有更好的兼容性
      type: 'binary',
    };
    const wbout = XLSX.write(workbook, wopts);
    const blob = new Blob([s2ab(wbout)], { type: 'application/octet-stream' });

    // 字符串转ArrayBuffer
    function s2ab(s: any) {
      const buf = new ArrayBuffer(s.length);
      const view = new Uint8Array(buf);
      for (let i = 0; i !== s.length; ++i) {
        view[i] = s.charCodeAt(i) & 0xff;
      }
      return buf;
    }

    return blob;
  };

  const onExportBasicExcel = () => {
    const excelData = [['Item', 'Status']];

    data['result'].forEach((item) => {
      const list = [item.item, 'Failed'];
      excelData.push(list);
    });
    const sheet = XLSX.utils.aoa_to_sheet(excelData);
    openDownloadDialog(sheet2blob(sheet, 'default'), 'test-details.xlsx');
  };

  // useEffect(() => {
  //   console.log('asdfasdfasdf', data, data.valueOf('result'));
  // }, [data]);

  return (
    <div>
      <Button
        type="primary"
        style={{ marginBottom: 10 }}
        onClick={onExportBasicExcel}
      >
        导出excel
      </Button>
      <div className="failed">
        <p style={{ fontSize: 30, margin: 0 }}>Slef Calibration Failed</p>
      </div>
      <Table
        id="antdTable"
        columns={columns}
        rowClassName={(record, i) => (i % 2 === 1 ? 'even' : 'odd')}
        dataSource={data['result']}
        scroll={{ x: 600 }}
        pagination={{ pageSize: 10 }}
      />
      {/* <ReactHTMLTableToExcel
        id="test-table-xls-button"
        className="download-table-xls-button"
        table="table-to-xls"
        filename="errorTable"
        sheet="default"
        buttonText="Download as XLS"
      /> */}
    </div>
  );
};

export default Result;
function saveAs(blob: Blob, fileName: string) {
  throw new Error('Function not implemented.');
}
