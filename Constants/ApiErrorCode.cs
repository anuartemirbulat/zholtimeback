using System;

namespace Constants
{
    public enum ApiErrorCode
    {
        UnknownError = -1,
        AuthenticationFailed = 1,
        UnauthorizedRequest = 2,
        Forbidden = 3,
        ValidationError = 5,
        ResourceNotFound = 7,
        InvalidToken = 9,
        ConnectionError = 11,
        ExternalTimeOutError = 12
    }
}