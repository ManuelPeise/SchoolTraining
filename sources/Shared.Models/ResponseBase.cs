namespace Shared.Models
{
    public class ResponseBase<TModel>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public TModel? Data { get; set; }
    }

    public class ResponseBase
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
