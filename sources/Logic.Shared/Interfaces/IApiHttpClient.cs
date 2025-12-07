using Shared.Models;

namespace Logic.Shared.Interfaces
{
    public interface IApiHttpClient
    {
        Task<ResponseBase<TModel>> GetAsync<TModel>(string requestUri);
        Task<ResponseBase<TModel>> PostAsync<TModel>(string requestUri, string body);
        Task<ResponseBase> PostAsync(string requestUri, string body);
        void EnsureAthenticationToken(string? token);
    }
}
