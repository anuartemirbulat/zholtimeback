using DataContracts.Base;

namespace Services.Interfaces;

public interface IWaygoHttpService
{
    Task<ApiResponse<TResponse>> SendAsync<TResponse>(HttpClient httpClient, string url, object request,
        HttpMethod method);

    Task<ApiResponse<bool>> SendAsync(HttpClient httpClient, string url, object request, HttpMethod method);
    Task<ApiResponse<TResponse>> SendEmptyAsync<TResponse>(HttpClient httpClient, string url, HttpMethod method);
    Task<ApiResponse<bool>> SendEmptyAsync(HttpClient httpClient, string url, HttpMethod method);
    Task<ApiResponse<T>> GetAsync<T>(HttpClient httpClient, UriBuilder uriBuilder);
}
