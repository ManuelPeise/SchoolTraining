namespace Shared.Models
{
    public class NotificationDataResponse<TModel>: NotificationResponse
    {
        public TModel? Data { get; set; }
    }
}
