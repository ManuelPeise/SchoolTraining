export interface IStatelessApi<TResponse, TRequest> {
  get(url: string, params?: TRequest): Promise<TResponse>;
  post(url: string, data: TRequest): Promise<TResponse>;
  postFile(url: string, file: File): Promise<TResponse>;
  downloadFile: (url: string, downloadFileName?: string) => Promise<void>;
}
