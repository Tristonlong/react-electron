/* eslint-disable camelcase */
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import zh_CN from 'antd/es/locale/zh_CN';
import { ConfigProvider } from 'antd';
import GlobalRouter from './GlobalRouter';
import store from './redux/store';

const root = createRoot(document!.getElementById('root')!);
root.render(
  <Provider store={store}>
    <ConfigProvider key="cn" locale={zh_CN}>
      <GlobalRouter />
    </ConfigProvider>
  </Provider>
);

// // calling IPC exposed from preload script
// window.electron.ipcRenderer.once('ipc-example', (arg) => {
//   // eslint-disable-next-line no-console
//   console.log(arg);
// });
// window.electron.ipcRenderer.sendMessage('ipc-example', ['ping']);
