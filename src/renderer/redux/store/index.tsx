/* eslint-disable import/no-named-as-default */
import { configureStore, combineReducers } from '@reduxjs/toolkit';
import testGlobal from '../reducer/globalSlice';
import index from '../../pages/index/redux';
import internalCalibration from '../../pages/internalCalibration/redux';
import externalCalibration from '../../pages/externalCalibration/redux';

export const reducer = combineReducers({
  globalRedux: testGlobal,
  index,
  internalCalibration,
  externalCalibration,
});

const store = configureStore({
  reducer,
});

export default store;

export type RootState = ReturnType<typeof reducer>;
export type AppDispatch = typeof store.dispatch;
