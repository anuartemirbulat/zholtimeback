using Constants;

namespace DataContracts.Exceptions;

public class RegRidesException : Exception
{
    public ApiErrorCode ApiErrorCode { get; }

    public RegRidesException(string message, ApiErrorCode apiErrorCode) : base(message)
    {
        ApiErrorCode = apiErrorCode;
    }

    public RegRidesException(string message) : this(message, ApiErrorCode.UnknownError)
    {
    }
}
