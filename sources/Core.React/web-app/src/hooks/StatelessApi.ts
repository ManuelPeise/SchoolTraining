import AxiosClient from './../lib/api/AxiosClient';
import { IStatelessApi } from 'src/lib/interfaces/IStatelessApi';

const createStatelessApi = <TResponse, TRequest>(): IStatelessApi<TResponse, TRequest> => {
  return {
    get: get,
    post: post,
  };
};

const get = async <TResponse, TRequest>(url: string, params?: TRequest): Promise<TResponse> => {
  return new Promise<TResponse>((resolve, reject) => {
    AxiosClient.get<TResponse>(url, { params })
      .then((response: any) => {
        resolve(response.data);
      })
      .catch((error: any) => {
        reject(error);
      });
  });
};

const post = async <TResponse, TRequest>(url: string, data: TRequest): Promise<TResponse> => {
  return new Promise<TResponse>((resolve, reject) => {
    AxiosClient.post<TResponse>(url, data)
      .then((response: any) => {
        resolve(response.data);
      })
      .catch((error: any) => {
        reject(error);
        console.error('StatelessApi POST error:', error);
      });
  });
};

export const StatelessApi = {
  create: createStatelessApi,
};
