using Constants.Enums;

namespace Identity.DataContract;

public class CurrentProfileResponse
{
    public string PhoneNumber { get; set; }
    public string InitialName { get; set; }
    public string NickName { get; set; }
    public RoleEnum Role { get; set; }
    public string RoleName { get; set; }
}
