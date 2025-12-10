using Data.Entities;
using Logic.Shared.Interfaces;
using MySqlX.XDevAPI.Common;
using Shared.Enums;
using Shared.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Logic.Shared
{
    public class ApiHttpClient : IApiHttpClient
    {
        private HttpClient _httpClient;
        private readonly ILogService _logService;
        private Uri _baseAddress { get; set; } = new Uri("http://localhost:5015/",UriKind.Absolute);

        public ApiHttpClient(ILogService logService)
        {
            _httpClient = new HttpClient();
            _logService = logService;
        }

        public async Task<ResponseBase<TModel>> GetAsync<TModel>(string requestUri)
        {
            try
            {
                var absoluteRequestUri = new Uri($"{_baseAddress}{requestUri}", UriKind.Absolute);

                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = absoluteRequestUri,
                    Version = new Version(1, 1)
                };

                var response = await _httpClient.SendAsync(requestMessage);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<TModel>(responseContent, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
                });

                return new ResponseBase<TModel>
                {
                    IsSuccess = true,
                    Data = result
                };
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Api request [{requestUri}] failed, {exception.Message}");

                await _logService.LogMessage(new LogMessageEntity
                {
                    Message = $"Api request [{requestUri}] failed.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    LogLevel = LogLevelEnum.Error,
                });

                return new ResponseBase<TModel>
                {
                    IsSuccess = false,
                    Message = $"GET Request data from api [{requestUri} failed.]",
                };
            }
        }

        public async Task<ResponseBase<TModel>> PostAsync<TModel>(string requestUri, string body, string? contentType = null)
        {
            try
            {
                var absoluteRequestUri = new Uri($"{_baseAddress}{requestUri}", UriKind.Absolute);

                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = absoluteRequestUri,
                    Version = new Version(1, 1),
                    Content = new StringContent(body, Encoding.UTF8, contentType ?? "application/json")
                };

                var response = await _httpClient.SendAsync(requestMessage);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(responseContent))
                {
                    return new ResponseBase<TModel>
                    {
                        IsSuccess = true,
                    };
                }
                else
                {
                    var result = JsonSerializer.Deserialize<TModel>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
                    });

                    return new ResponseBase<TModel>
                    {
                        IsSuccess = true,
                        Data = result
                    };
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Api request [{requestUri}] failed, {exception.Message}");

                await _logService.LogMessage(new LogMessageEntity
                {
                    Message = $"Api request [{requestUri}] failed.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    LogLevel = LogLevelEnum.Error,
                });

                return new ResponseBase<TModel>
                {
                    IsSuccess = false,
                    Message = $"POST Request data from api [{requestUri} failed.]",
                };
            }
        }

        public async Task<ResponseBase> PostAsync(string requestUri, string body)
        {
            try
            {
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{_baseAddress}{requestUri}", UriKind.Absolute),
                    Version = new Version(1, 1),
                    Content = new StringContent(body, Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(requestMessage);

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                var result = bool.Parse(responseContent);

                return new ResponseBase
                {
                    IsSuccess = result
                };
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Api request [{requestUri}] failed, {exception.Message}");

                await _logService.LogMessage(new LogMessageEntity
                {
                    Message = $"Api request [{requestUri}] failed.",
                    ExeptionMessage = exception.Message,
                    StackTrace = exception.StackTrace,
                    LogLevel = LogLevelEnum.Error,
                });

                return new ResponseBase
                {
                    IsSuccess = false,
                    Message = $"POST Request data from api [{requestUri} failed.]",
                };
            }
        }

        public void EnsureAthenticationToken(string? token)
        {
            if(string.IsNullOrEmpty(token))
            {
                return;
            }

            if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
            }

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }
}
