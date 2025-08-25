using CastAmNow.Core.Dtos;
using CastAmNow.Core.Models;

namespace CastAmNow.Core.Services;

public interface IBackendApiService
{
    Task<Response<TResponse>> GetAsync<TResponse>(string url);
    Task<Response<TResponse>> PostAsync<TResponse, TRequest>(string url, TRequest payload, bool isFileUpload = false, string contentType = "application/json", bool skipAuth = false);
    Task<Response<T>> PutAsync<T, TRequest>(string url, TRequest payload);
    Task<Response<T>> DeleteAsync<T>(string url);
}