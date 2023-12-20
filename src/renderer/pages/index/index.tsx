/* eslint-disable consistent-return */
/* eslint-disable no-unneeded-ternary */
/* eslint-disable import/no-absolute-path */
/* eslint-disable react/no-unstable-nested-components */
/* eslint-disable react/jsx-props-no-spreading */
/* eslint-disable react/jsx-no-undef */
/* eslint-disable vars-on-top */
/* eslint-disable object-shorthand */
/* eslint-disable jsx-a11y/control-has-associated-label */
/* eslint-disable react/button-has-type */
/* eslint-disable react/jsx-curly-brace-presence */
/* eslint-disable default-case */
/* eslint-disable no-undef */
/* eslint-disable no-param-reassign */
/* eslint-disable prefer-template */
/* eslint-disable camelcase */
/* eslint-disable no-console */
/* eslint-disable prefer-destructuring */
/* eslint-disable no-var */
/* eslint-disable react-hooks/exhaustive-deps */
/* eslint-disable @typescript-eslint/no-shadow */
/* eslint-disable react/function-component-definition */
/* eslint-disable @typescript-eslint/no-unused-vars */

// import './index.less';

import PropTypes from 'prop-types';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { useEffect, useState } from 'react';
import {
  Button,
  Input,
  Modal,
  Select,
  Space,
  Upload,
  UploadFile,
  UploadProps,
} from 'antd';
import { AppDispatch } from '../../redux/store';
import InternalCalibration from '../internalCalibration';
import ExternalCalibration from '../externalCalibration';
import NewCalibration from '../newCalibration';
import './index.css';

const { TextArea } = Input;

// This function will output the lines from the script
// and will return the full combined output
// as well as exit code when it's done (using the callback).

const Index = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();

  const [calibrationType, setCalibrationType] = useState(1);

  return (
    <div style={{ height: window.innerHeight }}>
      <div className="navigationBar">
        <Button
          className="navigationBarBtn"
          onClick={() => {
            setCalibrationType(1);
          }}
          type={calibrationType === 1 ? 'primary' : 'text'}
          style={{ backgroundColor: calibrationType === 1 ? '#0c8918' : '' }}
        >
          Internal Calibration
        </Button>
        <Button
          className="navigationBarBtn"
          onClick={() => {
            setCalibrationType(2);
          }}
          type={calibrationType === 2 ? 'primary' : 'text'}
          style={{ backgroundColor: calibrationType === 2 ? '#0c8918' : '' }}
        >
          External Calibration
        </Button>
        <Button
          className="navigationBarBtn"
          onClick={() => setCalibrationType(3)}
          type={calibrationType === 3 ? 'primary' : 'text'}
        >
          New Calibration
        </Button>
      </div>
      <div>
        {calibrationType === 1 && <InternalCalibration />}
        {calibrationType === 2 && <ExternalCalibration />}
        {calibrationType === 3 && <NewCalibration />}
      </div>
    </div>
  );
};

export default Index;
