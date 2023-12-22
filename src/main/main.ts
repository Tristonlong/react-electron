/* eslint-disable array-callback-return */
/* eslint-disable prettier/prettier */
/* eslint-disable no-useless-concat */
/* eslint-disable camelcase */
/* eslint-disable @typescript-eslint/no-shadow */
/* eslint-disable no-restricted-syntax */
/* eslint-disable no-unused-expressions */
/* eslint-disable consistent-return */
/* eslint-disable dot-notation */
/* eslint-disable no-var */
/* eslint-disable vars-on-top */
/* eslint-disable no-plusplus */
/* eslint-disable prefer-template */
/* eslint-disable prefer-const */
/* eslint-disable no-array-constructor */
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint global-require: off, no-console: off, promise/always-return: off */

/**
 * This module executes inside of electron's main process. You can start
 * electron renderer process from here and communicate with the other processes
 * through IPC.
 *
 * When running `npm run build` or `npm run build:main`, this file is compiled to
 * `./src/main.js` using webpack. This gives us some performance wins.
 */
import path, { resolve } from 'path';
import { app, BrowserWindow, shell, ipcMain, dialog } from 'electron';
import { autoUpdater } from 'electron-updater';
import log from 'electron-log';
import cp from 'child_process';
import iconv from 'iconv-lite';
import MenuBuilder from './menu';
import { resolveHtmlPath } from './util';

function formatDate(date: Date, format: string) {
  const map: any = {
    mm: date.getMonth() + 1,
    dd: date.getDate(),
    yyyy: date.getFullYear().toString(),
    HH: date.getHours(),
    MM: date.getMinutes(),
    SS: date.getSeconds(),
  };

  return format.replace(
    /mm|dd|yyyy|HH|MM|SS/gi,
    (matched: any) => map[matched]
  );
}

class AppUpdater {
  constructor() {
    log.transports.file.level = 'info';
    autoUpdater.logger = log;
    autoUpdater.checkForUpdatesAndNotify();
  }
}

let mainWindow: BrowserWindow | null = null;

let mySpawn = new Array();

// 通用部分
ipcMain.on('read-version', async (e, msg) => {
  var fs = require('fs');
  var app_path = 'C:\\Program Files\\Testrong\\ATE_Tester\\ATE_Tester.exe';
  var app_offscreen =
    'C:\\Program Files\\Testrong\\ATE_Tester\\Testrong.OffScreen.exe';
  const { dialog } = require('electron');
  if (!fs.existsSync(app_path)) {
    dialog.showErrorBox('Cannot find ATE software', 'Please install ATE first');
    app.exit();
  }

  var version = iconv.decode(
    cp.execSync('"' + app_offscreen + '"' + ' --version'),
    'cp936'
  );
  e.sender.send('read-version-reply', version.split('\n')[0]);
});

ipcMain.on('kill', async (e, msg) => {
  if (mySpawn.length !== 0) {
    console.log('关闭进程 -> mainProcess:' + msg);

    e.sender.send('cs-reply', '正在关闭所有打开的应用');
    // 收到消息，关闭所有进程
    for (let i = 0; i < mySpawn.length; i++) {
      mySpawn[i].kill();
    }
    mySpawn = new Array();
  }
});

ipcMain.on('read-config', async (e, msg) => {
  if (mySpawn.length > 0) {
    console.log(dialog.showErrorBox('正在运行中', '已有一个自校验正在运行'));
    return;
  }
  var fs = require('fs');
  fs.readFile(msg['config'], (err: any, data: { toString: () => string }) => {
    if (err) {
      return console.error(err);
    }

    e.sender.send('read-config-reply', data.toString());
  });
});

ipcMain.on('read-stil', async (e, msg) => {
  var fs = require('fs');
  fs.readFile(msg, (err: any, data: { toString: () => string }) => {
    if (err) {
      return console.error(err);
    }
    e.sender.send('read-stil-reply', data.toString());
  });
});

ipcMain.on('search-path', async (e, msg) => {
  if (mySpawn.length > 0) {
    console.log(dialog.showErrorBox('正在运行中', '已有一个自校验正在运行'));
    return;
  }
  var data = cp.execSync(msg['cmd']);
  e.sender.send('search-path-reply', iconv.decode(data, 'cp936'));
});

// internal calibration进程通信
ipcMain.on('write-config-json', async (e, msg) => {
  if (mySpawn.length > 0) {
    console.log(dialog.showErrorBox('正在运行中', '已有一个自校验正在运行'));
    return;
  }

  var fs = require('fs');
  var originData = fs.readFileSync(msg['file']);

  originData = JSON.parse(originData);

  // originData.Configurations[0].CompilePattern = 'ForceAndCompile';
  // }
  originData.Configurations[0].STILPath = resolve('./temp.stil');
  originData.Configurations[0].DatalogFolder = resolve('./DatalogFolder');
  originData.Configurations[0].Mode = 'Production';

  fs.writeFileSync('temp.json', JSON.stringify(originData), (error: any) => {
    // console.log(error);
  });
});

ipcMain.on('cover-tcp', async (e, msg) => {
  if (mySpawn.length > 0) {
    console.log(dialog.showErrorBox('正在运行中', '已有一个自校验正在运行'));
    return;
  }

  let newTCPData = msg['tcps']
    .map((value: any, index: number) => {
      return (
        value.name +
        '\t' +
        value.ip +
        '\t' +
        value.port +
        '\t' +
        value.timeout +
        '\t' +
        value.enabled +
        ';\n'
      );
    })
    .join('');
  newTCPData = 'TCP {\n' + newTCPData + '}\n';
  var fs = require('fs');
  var originData = fs.readFileSync(msg['filePath']);

  const block = /TCP \{[^}]*\}/g;
  var newData = originData.toString().replace(block, newTCPData);
  fs.writeFileSync('temp.stil', newData, (error: any) => {
    console.log(error);
  });

  // delete old one
  const originPath = resolve('./DatalogFolder');
  const oldPath = resolve('./DatalogFolder-old');

  if (!fs.existsSync(originPath)) {
    fs.mkdirSync(originPath);
    fs.mkdirSync(oldPath);
  } else {
    var newFolder =
      oldPath + '/' + String(formatDate(new Date(), 'yyyy-dd-mm_HH-MM-SS'));
    fs.mkdirSync(newFolder);
    fs.readdir(originPath, (err: any, files: any) => {
      for (let file of files) {
        const originalFile = resolve(originPath, file);
        const targetPathFile = resolve(newFolder, file);

        fs.rename(originalFile, targetPathFile, (err: any) => {
          console.log(err);
        });
      }
    });
  }

  var spawn = cp.spawn(msg['cmd'], ['--Run', resolve('./temp.json')]);
  spawn.stderr.on('data', (data) => {
    console.log('Error:', data);
  });

  spawn.stdout.on('data', (data) => {
    e.sender.send('cp-reply', iconv.decode(data, 'cp936'));
  });
  mySpawn.push(spawn);
});

ipcMain.on('run-parse-result', async (e, msg) => {
  // parse result
  var files: string = iconv.decode(
    cp.execSync('dir /b ' + resolve('./DatalogFolder')),
    'cp936'
  );
  var stilFile = files
    .split('\r\n')
    .filter((value: string) => value && value.indexOf('.stdf') !== -1)[0];
  const goldenFile = msg['mode'].trim() === 'dc' ? 'DC_golden' : 'K7_golden';
  const cmd =
    'cd ' +
    resolve('./ParseAndValidation') +
    '&&' +
    resolve('./ParseAndValidation/parse.exe') +
    ' --file ' +
    resolve('./DatalogFolder/' + stilFile) +
    ' --golden ' +
    resolve('./ParseAndValidation/goldenFiles/' + goldenFile) +
    ' --mode ' +
    msg['mode'];

  try {
    cp.execSync(cmd);
    // read result

    var fs = require('fs');
    var data = fs.readFileSync(
      resolve('./ParseAndValidation/results/result.json')
    );

    e.sender.send('run-parse-result-reply', JSON.parse(data));

    if (fs.existsSync(resolve('./ParseAndValidation/results/result.json'))) {
      fs.readdir(
        resolve('./ParseAndValidation/results/'),
        (err: any, files: any) => {
          for (let file of files) {
            const originalFile = resolve('./ParseAndValidation/results/', file);
            const targetPathFile = resolve(
              './ParseAndValidation/results/',
              file.indexOf('old') === -1 ? file + '.old' : file
            );
            fs.rename(originalFile, targetPathFile, (err: any) => {
              console.log(err);
            });
          }
        }
      );
    }
  } catch (err: any) {
    e.sender.send(
      'run-parse-result-reply',
      JSON.parse(
        JSON.stringify({
          Error: iconv.decode(err.stderr, 'cp936'),
          // Error: err,
        })
      )
    );
  }
});

// external calibration进程通信
ipcMain.on('pre-test', async (e, msg) => {
  // 一次性只执行一次
  if (mySpawn.length > 0) {
    return;
  }
 
  var fs = require('fs');
  const stilFilePath1 = 'D:\\project\\calibrationtool-dev_0.9.1\\SW1.2_55_calibration_RZ\\selfCalibration\\Calibration_DC_Reset.stil';
  const stilFilePath2 = 'D:\\project\\calibrationtool-dev_0.9.1\\SW1.2_55_calibration_RZ\\selfCalibration\\Calibration_DC_MeasRload.stil';
   // 读取第一个STIL文件
   var stilData1 = fs.readFileSync(stilFilePath1);
    // 读取第二个STIL文件
  var stilData2 = fs.readFileSync(stilFilePath2);
  // 修改tcp
  let newTCPData = msg['tcps']
    .map((value: any, index: number) => {
      return (
        value.name +
        '\t' +
        value.ip +
        '\t' +
        value.port +
        '\t' +
        value.timeout +
        '\t' +
        value.enabled +
        ';\n'
      );
    })
    .join('');
  newTCPData = 'TCP {\n' + newTCPData + '}\n';
  var originData = fs.readFileSync(msg['filePath']);

  // change user program path
  var userProgramPath = resolve(
    '.\\sw_info\\selfCalibration\\Testrong.User.Program\\bin\\Debug\\UserProgram.dll'
  );
  const userProgramBlock = /TestrongCodePath=[^;]*/g;
  var newData = originData
    .toString()
    .replace(userProgramBlock, 'TestrongCodePath=' + userProgramPath);

  // change TCP settings
  const tcpBlock = /TCP \{[^}]*\}/g;
  newData = newData.toString().replace(tcpBlock, newTCPData);
  fs.writeFileSync('temp.stil', newData, (error: any) => {
    console.log(error);
  });

  // 清理存放结果的路径
  const originPath = resolve('./DatalogFolder');
  const oldPath = resolve('./DatalogFolder-old');

  if (!fs.existsSync(originPath)) {
    fs.mkdirSync(originPath);
    fs.mkdirSync(oldPath);
  } else {
    var newFolder =
      oldPath + '/' + String(formatDate(new Date(), 'yyyy-dd-mm_HH-MM-SS'));
    fs.mkdirSync(newFolder);
    fs.readdir(originPath, (err: any, files: any) => {
      for (let file of files) {
        const originalFile = resolve(originPath, file);
        const targetPathFile = resolve(newFolder, file);

        fs.rename(originalFile, targetPathFile, (err: any) => {
          console.log(err);
        });
      }
    });
  }

  // 清理self calibration 可能存在的残留csv文件
  cp.execSync(
    'del C:\\ProgramData\\Testrong\\ATE_Tester\\SelfCalibration\\*.csv'
  );

  // 运行第一次calibration
  var app_path = 'C:\\Program Files\\Testrong\\ATE_Tester\\ATE_Tester.exe';
  const { dialog } = require('electron');
  if (!fs.existsSync(app_path)) {
    dialog.showErrorBox('Cannot find ATE software', 'Please install ATE first');
    app.exit();
  }
  var spawn = cp.spawn(msg['cmd'], ['--Run', resolve('./temp.json')]);

  // 错误输出
  spawn.stderr.on('data', (data) => {
    console.log('Error:', data);
  });
  // 正常输出
  spawn.stdout.on('data', (data) => {
    e.sender.send('pre-test-reply', iconv.decode(data, 'cp936'));
  });
  // 进程结束hook
  spawn.on('close', (code) => {
    e.sender.send('pre-test-reply', 'End: ' + String(code!));
  });
  mySpawn.push(spawn);
});

// 执行线性回归
ipcMain.on('linear-regression', async (e, msg) => {
  var fs = require('fs');
  var config = fs.readFileSync(resolve('./linearRegression/config.json'));
  // doing linear regression
  var code: string = iconv.decode(
    cp.execSync(
      'cd ./linearRegression && python ' +
        resolve('./linearRegression/linearRegression.py')
    ),
    'cp936'
  );
  // 执行的python程序，正常情况下不会有输出
  if (code !== '') {
    e.sender.send('linear-regression-reply', { ret: -1 });
    // 对python脚本的输出
    e.sender.send("linear-regression-reply",{ret:code === ''? 0 : -1,output:code,config})
  } else {
    e.sender.send('linear-regression-reply', { ret: 0, config });
  }
});

ipcMain.on('post-test', async (e, msg) => {
  // 一次性只执行一次
  if (mySpawn.length > 0) {
    return;
  }

  var fs = require('fs');
  const newStilFilePath = 'D:\\project\\calibrationtool-dev_0.9.1\\SW1.2_55_calibration_RZ\\selfCalibration\\Calibration_DC_DataUpload.stil'
  var newStilData = fs.readFileSync(newStilFilePath);
  // 修改tcp
  let newTCPData = msg['tcps']
    .map((value: any, index: number) => {
      return (
        value.name +
        '\t' +
        value.ip +
        '\t' +
        value.port +
        '\t' +
        value.timeout +
        '\t' +
        value.enabled +
        ';\n'
      );
    })
    .join('');
  newTCPData = 'TCP {\n' + newTCPData + '}\n';
  var originData = fs.readFileSync(msg['filePath']);

  var userProgramPath = resolve(
    '.\\sw_info\\selfCalibration\\Testrong.User.Program\\bin\\Debug\\UserProgram.dll'
  );
  const userProgramBlock = /TestrongCodePath=[^;]*/g;
  var newData = originData
    .toString()
    .replace(userProgramBlock, 'TestrongCodePath=' + userProgramPath);

  // change TCP settings
  const tcpBlock = /TCP \{[^}]*\}/g;
  newData = newData.toString().replace(tcpBlock, newTCPData);
  fs.writeFileSync('temp.stil', newData, (error: any) => {
    console.log(error);
  });

  // 清理存放结果的路径
  const originPath = resolve('./DatalogFolder');
  const oldPath = resolve('./DatalogFolder-old');

  if (!fs.existsSync(originPath)) {
    fs.mkdirSync(originPath);
    fs.mkdirSync(oldPath);
  } else {
    var newFolder =
      oldPath + '/' + String(formatDate(new Date(), 'yyyy-dd-mm_HH-MM-SS'));
    fs.mkdirSync(newFolder);
    fs.readdir(originPath, (err: any, files: any) => {
      for (let file of files) {
        const originalFile = resolve(originPath, file);
        const targetPathFile = resolve(newFolder, file);

        fs.rename(originalFile, targetPathFile, (err: any) => {
          console.log(err);
        });
      }
    });
  }

  // 运行带有offset的calibration
  var app_path = 'C:\\Program Files\\Testrong\\ATE_Tester\\ATE_Tester.exe';
  const { dialog } = require('electron');
  if (!fs.existsSync(app_path)) {
    dialog.showErrorBox('Cannot find ATE software', 'Please install ATE first');
    app.exit();
  }
  var spawn = cp.spawn(msg['cmd'], ['--Run', resolve('./temp.json')]);

  // 错误输出
  spawn.stderr.on('data', (data) => {
    console.log('Error:', data);
  });
  // 正常输出
  spawn.stdout.on('data', (data) => {
    e.sender.send('post-test-reply', iconv.decode(data, 'cp936'));
  });
  // 进程结束hook
  spawn.on('close', (code) => {
    e.sender.send('post-test-reply', 'End: ' + String(code!));
  });
  mySpawn.push(spawn);
});

// 执行结果解析
ipcMain.on('parse-post-test', async (e, msg) => {
  var fs = require('fs');
  var config = fs.readFileSync(resolve('./ResultExport/config.json'));
  // doing linear regression
  var code: string = iconv.decode(
    cp.execSync(
      'cd ./ResultExport && python ' + resolve('./ResultExport/ResultExport.py')
    ),
    'cp936'
  );
  // 执行的python程序，正常情况下不会有输出
  if (code !== '') {
    e.sender.send('parse-post-test-reply', { ret: -1 });
  } else {
    fs = require('fs');
    var res = fs.readFileSync(
      resolve('./ResultExport/results/calibration_state.csv')
    );
    var resArr: any[] = res.toString().split('\r\n');
    let filtedRes = resArr.filter((value: any, index: number) => {
      var val = value.split(',');
      return val[val.length - 1] === 'False';
    });
    let resizedRes = filtedRes.map((value: any, index: number) => {
      var val = value.split(',');
      return { item: val[0] };
    });

    e.sender.send('parse-post-test-reply', { ret: 0, res: resizedRes });
  }
});
// 扫码枪结果
ipcMain.on('save-scanned-code',async (e, scannedCode) =>{
  var fs = require('fs');
  const filePath = path.join("C:\\","ProgramData\\Testrong\\ATE_Tester\\TestResult","BoardInfo.txt")
  fs.appendFile(filePath,scannedCode + '\n', (err:any) =>{
    if(err){
      console.log('Error writing file', err);
    }else {
      console.log('Successfully wrote scanned code to file');
    }
  })
})

// 通信例子
ipcMain.on('ipc-example', async (event, arg) => {
  const msgTemplate = (pingPong: string) => `IPC test: ${pingPong}`;
  console.log(msgTemplate(arg));
  event.reply('ipc-example', msgTemplate('pong'));
});

if (process.env.NODE_ENV === 'production') {
  const sourceMapSupport = require('source-map-support');
  sourceMapSupport.install();
}

const isDebug =
  process.env.NODE_ENV === 'development' || process.env.DEBUG_PROD === 'true';

if (isDebug) {
  require('electron-debug')();
}

const installExtensions = async () => {
  const installer = require('electron-devtools-installer');
  const forceDownload = !!process.env.UPGRADE_EXTENSIONS;
  const extensions = ['REACT_DEVELOPER_TOOLS'];

  return installer
    .default(
      extensions.map((name) => installer[name]),
      forceDownload
    )
    .catch(console.log);
};

const createWindow = async () => {
  if (isDebug) {
    await installExtensions();
  }

  const RESOURCES_PATH = app.isPackaged
    ? path.join(process.resourcesPath, 'assets')
    : path.join(__dirname, '../../assets');

  const getAssetPath = (...paths: string[]): string => {
    return path.join(RESOURCES_PATH, ...paths);
  };

  mainWindow = new BrowserWindow({
    show: false,
    width: 1024,
    height: 1024,
    icon: getAssetPath('icon.png'),
    webPreferences: {
      preload: app.isPackaged
        ? path.join(__dirname, 'preload.js')
        : path.join(__dirname, '../../.erb/dll/preload.js'),
      nodeIntegration: true,
      nodeIntegrationInWorker: true,
      // contextIsolation: false,
    },
  });

  mainWindow.loadURL(resolveHtmlPath('index.html'));

  mainWindow.on('ready-to-show', () => {
    if (!mainWindow) {
      throw new Error('"mainWindow" is not defined');
    }
    if (process.env.START_MINIMIZED) {
      mainWindow.minimize();
    } else {
      mainWindow.show();
    }
  });

  mainWindow.on('closed', () => {
    mainWindow = null;
  });

  const menuBuilder = new MenuBuilder(mainWindow);
  menuBuilder.buildMenu();

  // Open urls in the user's browser
  mainWindow.webContents.setWindowOpenHandler((edata) => {
    shell.openExternal(edata.url);
    return { action: 'deny' };
  });

  mainWindow.maximize();

  // Remove this if your app does not use auto updates
  // eslint-disable-next-line
  new AppUpdater();
};

/**
 * Add event listeners...
 */

app.on('window-all-closed', () => {
  // Respect the OSX convention of having the application in memory even
  // after all windows have been closed
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

app
  .whenReady()
  .then(() => {
    createWindow();
    app.on('activate', () => {
      // On macOS it's common to re-create a window in the app when the
      // dock icon is clicked and there are no other windows open.
      if (mainWindow === null) createWindow();
    });
  })
  .catch(console.log);
