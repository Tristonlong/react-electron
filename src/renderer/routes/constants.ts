/* eslint-disable import/prefer-default-export */
import { IFMenuBase } from './types';

const ROUTE_LIST: IFMenuBase[] = [
  // 菜单相关路由
  {
    key: '/',
    title: 'index',
    component: 'index',
  },
  {
    key: '/settings',
    title: 'settings',
    component: 'settings',
  },
];

export default ROUTE_LIST;
