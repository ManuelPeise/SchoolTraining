export interface IStatelessApi<TResponse, TRequest> {
  get(url: string, params?: TRequest): Promise<TResponse>;
  post(url: string, data: TRequest): Promise<TResponse>;
}
