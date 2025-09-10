using Constants;

namespace DataContracts.Base;

public class ApiError
{
    public ApiErrorCode Code { get; }
    public string Text { get; }

    public ApiError(ApiErrorCode code, string text)
    {
        Code = code;
        Text = text;
    }

    public string ToMessage()
    {
        return $"{this.Code} - {this.Text}";
    }
}
