import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  testGlobal: 0,
};

export const { reducer, actions } = createSlice({
  name: 'searchBox',
  initialState,
  reducers: {
    testGlobal(state, action: any) {
      state.testGlobal = action.payload.testGlobal;
    },
  },
});

export const { testGlobal } = actions;

export default reducer;
