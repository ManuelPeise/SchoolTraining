import axios, { AxiosInstance } from 'axios';
import { ITokenStore } from '../interfaces/ITokenStore';

const AxiosClient: AxiosInstance = axios.create({
  baseURL: process.env.REACT_APP_API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor
AxiosClient.interceptors.request.use(
  (config) => {
    const tokenstore = localStorage.getItem('jwt');

    if (tokenstore) {
      const tokenStore: ITokenStore = JSON.parse(tokenstore);

      if (tokenStore.jwt && config.headers) {
        config.headers.Authorization = `Bearer ${tokenStore.jwt}`;
      }
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor
AxiosClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('jwt');
      window.location.href = '/auth';
    }
    return Promise.reject(error);
  }
);

export default AxiosClient;
