import axios, { AxiosInstance } from 'axios';

const AxiosClient: AxiosInstance = axios.create({
  baseURL: process.env.REACT_APP_API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor
AxiosClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('jwt');
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
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
