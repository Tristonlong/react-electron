import { Route, Navigate, Routes } from 'react-router-dom';
import ROUTE_LIST from './constants';
import { IFMenuBase } from './types';
import Pages from './pages';

function CRouter() {
  return (
    <Routes>
      {ROUTE_LIST.map((r: IFMenuBase) => {
        const Component = r.component && Pages[r.component];
        return (
          <Route
            key={r.key}
            path={r.key}
            element={
              <div title={r.title}>
                <Component />
              </div>
            }
          />
        );
      })}
      <Route element={<Navigate to="/404" />} />
    </Routes>
  );
}

export default CRouter;
