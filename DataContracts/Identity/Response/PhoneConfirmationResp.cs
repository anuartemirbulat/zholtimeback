namespace DataContracts.Identity.Response;

public class PhoneConfirmationResp
{
    public bool IsSuccess { get; set; }
    public Guid Guid { get; set; }
    private string ErrMessage { get; set; }

    public PhoneConfirmationResp(bool isSuccess, Guid guid, string errMessage="")
    {
        this.IsSuccess = isSuccess;
        this.Guid = guid;
        this.ErrMessage = errMessage;
    }
}