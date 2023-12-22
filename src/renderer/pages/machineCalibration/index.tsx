// import { useEffect, useState } from 'react';
// import { Button, Input, Modal } from 'antd';
// import SomeCustomCard from 'renderer/templates/SomeCustomCard';
// import Details from 'renderer/templates/details';
// import { useDispatch, useSelector } from 'react-redux';
// import {
//   getApp,
//   getCmd,
//   // 更多选择器...
// } from './redux/selectors';
// import {
//   setApp,
//   setCmd,
//   // 更多操作...
// } from './redux';
// import Settings from '../settings';

// const { TextArea } = Input;

// const NewCalibration = () => {
//   const dispatch = useDispatch();
//   // 使用useSelector获取Redux store中的状态
//   const app = useSelector(getApp);
//   const cmd = useSelector(getCmd);
//   // 更多状态...

//   const [isModalOpen, setIsModalOpen] = useState(false);
//   const [loadStatus, setLoadStatus] = useState(false);
//   const [showMsg, setShowMsg] = useState('');
//   const [log, setLog] = useState('');

//   // 添加更多状态和逻辑...

//   // 用于处理模态框的函数
//   const showModal = () => {
//     setIsModalOpen(true);
//   };

//   const handleOk = () => {
//     setIsModalOpen(false);
//     // 添加启动新校准的逻辑...
//   };

//   const handleCancel = () => {
//     setIsModalOpen(false);
//   };

//   // 添加更多效果和逻辑...

//   // 渲染组件
//   return (
//     <div style={{ height: window.innerHeight }}>
//       <Modal
//         title="Custom Modal for New Calibration"
//         open={isModalOpen}
//         onOk={handleOk}
//         onCancel={handleCancel}
//         width={700}
//       >
//         <Settings /* Props as needed */ />
//       </Modal>
//       <div>
//         <SomeCustomCard
//           title={'New Calibration Settings'}
//           // 添加需要的props
//           width={550}
//         />
//         <div style={{ margin: 18, textAlign: 'left' }}>
//           <Button type={'primary'} onClick={/* 添加启动新校准的函数 */}>
//             Start New Calibration
//           </Button>
//           <Button
//             style={{ marginLeft: 8 }}
//             danger
//             type={'primary'}
//             onClick={/* 添加停止校准的函数 */}
//           >
//             Stop New Calibration
//           </Button>
//         </div>

//         <Details
//           title={'Results:'}
//           msg={showMsg}
//           loadStatus={loadStatus}
//           // 更多props...
//         />

//         <div style={{ margin: 8 }}>
//           <p>log info: </p>
//           <TextArea
//             id="area"
//             showCount
//             autoSize={{ minRows: 10, maxRows: 10 }}
//             style={{ marginBottom: 24 }}
//             value={log}
//             placeholder="disable resize"
//           />
//         </div>
//       </div>
//     </div>
//   );
// };

// export default NewCalibration;
