import { createSlice } from '@reduxjs/toolkit';

export interface IAppIndexState {}

const initialState: IAppIndexState = {};

export const { reducer, actions } = createSlice({
  name: 'app/settings',
  initialState,
  reducers: {},
});

export const initType = initialState;

export default reducer;
