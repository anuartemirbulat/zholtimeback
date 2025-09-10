namespace DataContracts.Identity.Requests;

public class PhoneConfirmationRequest
{
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
}
