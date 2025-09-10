using DataContracts.Identity.Requests;
using DataContracts.Identity.Response;
using Identity.DAL.Entities.Entities;

namespace Identity.Services.Interfaces;

public interface IAuthService
{
    Task<JwtTokenResponse> Authenticate(AuthRequest request);
    Task<JwtTokenResponse> Authenticate(string phoneNumber,Guid guid);
    string GenerateAccessToken(Profile profile);
}
