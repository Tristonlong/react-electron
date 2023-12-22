// Disable no-unused-vars, broken for spread args
/* eslint no-unused-vars: off */
import { contextBridge, ipcRenderer, IpcRendererEvent } from 'electron';

export type Channels =
  | 'ipc-example'
  | 'read-version'
  | 'read-version-reply'
  | 'save-scanned-code'
  | 'kill'
  | 'cp-reply'
  | 'read-config'
  | 'read-config-reply'
  | 'read-stil'
  | 'read-stil-reply'
  | 'search-path'
  | 'search-path-reply'
  | 'read-stil'
  | 'read-stil-reply'
  | 'cover-tcp'
  | 'write-config-json'
  | 'run-parse-result'
  | 'run-parse-result-reply'
  // external calibration
  | 'pre-test'
  | 'pre-test-reply'
  | 'linear-regression'
  | 'linear-regression-reply'
  | 'post-test'
  | 'post-test-reply'
  | 'parse-post-test'
  | 'parse-post-test-reply';

const electronHandler = {
  ipcRenderer: {
    sendMessage(channel: Channels, ...args: unknown[]) {
      ipcRenderer.send(channel, ...args);
    },
    on(channel: Channels, func: (...args: unknown[]) => void) {
      const subscription = (_event: IpcRendererEvent, ...args: unknown[]) =>
        func(...args);
      ipcRenderer.on(channel, subscription);

      return () => {
        ipcRenderer.removeListener(channel, subscription);
      };
    },
    once(channel: Channels, func: (...args: unknown[]) => void) {
      ipcRenderer.once(channel, (_event, ...args) => func(...args));
    },
  },
};

contextBridge.exposeInMainWorld('electron', electronHandler);

export type ElectronHandler = typeof electronHandler;
