using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CastAmNow.Core.Abstractions;
using CastAmNow.Core.Dtos;

namespace CastAmNow.Core.Services;

public class BackendApiService(HttpClient httpClient, ILocalStorageService localStorage)
{
    private async Task SetAuthorizationHeader()
    {
        var token = await localStorage.GetItemAsStringAsync("authToken");

        httpClient.DefaultRequestHeaders.Authorization = 
            !string.IsNullOrWhiteSpace(token) ? 
                new AuthenticationHeaderValue("Bearer", token) : 
                null;
    }
    
    public async Task<ApiResponse<TResponse>> GetAsync<TResponse>(string url)
    {
        await SetAuthorizationHeader();
        var response = await httpClient.GetAsync(url);

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponse<TResponse>> PostAsync<TResponse, TRequest>(string url, TRequest payload, bool isFileUpload = false,string contentType = "application/json", bool skipAuth = false)
    {
        if (!skipAuth)
        {
            await SetAuthorizationHeader();
        }
        HttpContent content;
        var response = new HttpResponseMessage();
        if (isFileUpload && payload is MultipartFormDataContent multipartContent)
        {
            content = multipartContent;
        }
        else
        {
            var json = JsonSerializer.Serialize(payload);
            Console.WriteLine(json);
            content = new StringContent(json, Encoding.UTF8, contentType);
        }

        try
        {
            response = await httpClient.PostAsync(url, content);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return await ProcessResponse<TResponse>(response);
    }

    public async Task<ApiResponse<T>> PutAsync<T, TRequest>(string url, TRequest payload)
    {
        await SetAuthorizationHeader();
        var response = await httpClient.PutAsJsonAsync(url, payload);

        return await ProcessResponse<T>(response);
    }

    public async Task<ApiResponse<T>> DeleteAsync<T>(string url)
    {
        await SetAuthorizationHeader();
        var response = await httpClient.DeleteAsync(url);

        return await ProcessResponse<T>(response);
    }

    private static async Task<ApiResponse<T>> ProcessResponse<T>(HttpResponseMessage response)
    {
        var apiResponse = new ApiResponse<T>
        {
            IsSuccess = response.IsSuccessStatusCode,
            Message = response.ReasonPhrase
        };

        if (response.IsSuccessStatusCode)
        {
            apiResponse.Data = await response.Content.ReadFromJsonAsync<T>();
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            apiResponse.Message = !string.IsNullOrEmpty(errorMessage) ? errorMessage : response.ReasonPhrase;
        }

        return apiResponse;
    }
}