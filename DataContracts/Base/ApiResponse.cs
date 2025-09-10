using System.ComponentModel;
using Constants;
using Newtonsoft.Json;

namespace DataContracts.Base;

public class ApiResponse
{
    [DefaultValue(null)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public ApiError Error { get; set; }

    private ApiResponse(ApiError apiError)
    {
        Error = apiError;
    }

    protected ApiResponse()
    {
    }

    public bool HasError()
    {
        return Error != null;
    }

    public bool HasError(ApiErrorCode code)
    {
        return Error != null && Error.Code == code;
    }

    public static ApiResponse Failed(ApiErrorCode code, string text)
    {
        return new ApiResponse(new ApiError(code, text));
    }


    public static ApiResponse Failed(ApiErrorCode code, string text, int detailedErrorCode)
    {
        return new ApiResponse(new ApiError(code, text));
    }

    public static ApiResponse<T> Failed<T>(ApiErrorCode code, string text)
    {
        return new ApiResponse<T>() { Error = new ApiError(code, text) };
    }

    public static ApiResponse<T> Success<T>(T data)
    {
        return new ApiResponse<T>(data);
    }
}

public class ApiResponse<T> : ApiResponse
{
    public T Data { get; set; }

    public ApiResponse(T data)
    {
        Data = data;
    }

    public ApiResponse()
    {
    }

    public static implicit operator T(ApiResponse<T> result)
    {
        return result.Data;
    }
}
