using Constants.Enums;

namespace DataContracts.Identity.Requests;

public class RegistrationRequest
{
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public RoleEnum RoleEnum { get; set; }
}
