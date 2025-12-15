export interface INotificationDataResponse<TModel> {
  success: boolean;
  ResourceKey: string;
  Data: TModel;
}
