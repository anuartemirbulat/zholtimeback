using Constants.Enums;

namespace DataContracts.Identity.Requests;

public class AuthRequest
{
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public RoleEnum RoleEnum { get; set; }
}
