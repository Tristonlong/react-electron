/* eslint-disable react/function-component-definition */
/* eslint-disable import/no-anonymous-default-export */
import React from 'react';
import { HashRouter as Router } from 'react-router-dom';

import CRouter from './routes';

export default () => (
  <Router>
    {/* <Routes>
      <Route path="/*" element={<App />} />
      <Route path="/404" element={<NotFound />} />
      <Route element={<NotFound />} />
    </Routes> */}
    <CRouter />
  </Router>
);
