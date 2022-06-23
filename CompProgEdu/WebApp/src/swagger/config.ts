import axios from 'axios';
import { serviceOptions } from './index';

export const instance = axios.create({
  baseURL: process.env.REACT_APP_API_BASE_URL,
});

serviceOptions.axios = instance;
