export interface INotificationDataResponse<TModel> {
  success: boolean;
  resourceKey: string;
  data: TModel;
}
