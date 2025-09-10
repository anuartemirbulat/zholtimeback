using Constants;

namespace DataContracts.Exceptions;

public class IdentityException : Exception
{
    public ApiErrorCode ApiErrorCode { get; }

    public IdentityException(string message, ApiErrorCode apiErrorCode) : base(message)
    {
        ApiErrorCode = apiErrorCode;
    }

    public IdentityException(string message) : this(message, ApiErrorCode.UnknownError)
    {
    }
}
