using System.Text;
using Constants;
using DataContracts.Base;
using Services.Interfaces;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Services.Implementations;

public class WaygoHttpService : IWaygoHttpService
{
    public async Task<ApiResponse<TResponse>> SendAsync<TResponse>(HttpClient httpClient, string url,
        object request, HttpMethod method)
    {
        using var todoItemJson =
            new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        using var message = new HttpRequestMessage(method, url) { Content = todoItemJson, };
        try
        {
            using var httpResponse = await httpClient.SendAsync(message);
            if (httpResponse.IsSuccessStatusCode)
            {
                var tResponse = await this.ReadHttpResponseMessage<TResponse>(httpResponse);
                return ApiResponse.Success(tResponse);
            }

            var responseJson = await httpResponse.Content.ReadAsStringAsync();
            return ApiResponse.Failed<TResponse>(ApiErrorCode.UnknownError, responseJson);
        }
        catch (Exception e)
        {
            return ApiResponse.Failed<TResponse>(ApiErrorCode.ConnectionError, $"Сервис {url} недоступен");
        }
    }

    public async Task<ApiResponse<bool>> SendAsync(HttpClient httpClient, string url, object request,
        HttpMethod method)
    {
        var json = JsonSerializer.Serialize(request);
        using var todoItemJson = new StringContent(json, Encoding.UTF8, "application/json");
        using var message = new HttpRequestMessage(method, url) { Content = todoItemJson };
        try
        {
            using var response = await httpClient.SendAsync(message);
            var responseJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Пришел ответ от {url}: {responseJson}");
            if (response.IsSuccessStatusCode)
                return ApiResponse.Success(true);

            return ApiResponse.Failed<bool>(ApiErrorCode.UnknownError, responseJson);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return ApiResponse.Failed<bool>(ApiErrorCode.ConnectionError, $"Сервис {url} недоступен");
        }
    }

    public async Task<ApiResponse<TResponse>> SendEmptyAsync<TResponse>(HttpClient httpClient, string url,
        HttpMethod method)
    {
        using var message = new HttpRequestMessage(method, url);
        using var httpResponse = await httpClient.SendAsync(message);
        var tResponse = await this.ReadHttpResponseMessage<TResponse>(httpResponse);
        return ApiResponse.Success(tResponse);
    }

    protected async Task<TResponse> ReadHttpResponseMessage<TResponse>(HttpResponseMessage message)
    {
        var responseJsonString = await message.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<TResponse>(responseJsonString);
        return resp;
    }

    public async Task<ApiResponse<T>> GetAsync<T>(HttpClient httpClient, UriBuilder uriBuilder)
    {
        T data;
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync(uriBuilder.Uri);
            using HttpContent content = response.Content;
            var d = await content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode && d != null)
            {
                if (d != null)
                {
                    data = JsonConvert.DeserializeObject<T>(d);
                    return ApiResponse.Success((T)data);
                }
            }

            return ApiResponse.Failed<T>(ApiErrorCode.UnknownError, d);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<ApiResponse<bool>> SendEmptyAsync(HttpClient httpClient, string url, HttpMethod method)
    {
        using var message = new HttpRequestMessage(method, url);
        using var httpResponse = await httpClient.SendAsync(message);
        return httpResponse.IsSuccessStatusCode
            ? ApiResponse.Success(true)
            : ApiResponse.Failed<bool>(ApiErrorCode.UnknownError, httpResponse.ReasonPhrase);
    }
}