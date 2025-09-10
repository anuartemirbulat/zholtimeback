using DataContracts.Identity.Response;
using Identity.DataContract;

namespace Identity.Services.Interfaces;

public interface IProfilesService
{
    Task<CurrentProfileResponse> GetCurrentProfileInfo();
    Task<JwtTokenResponse> ChangeRole();
    Task<bool> Delete();
}
