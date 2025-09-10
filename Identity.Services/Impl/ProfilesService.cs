using Constants.Enums;
using Core;
using DataContracts.Identity.Response;
using DataContracts.Messages;
using Identity.DAL;
using Identity.DataContract;
using Identity.Services.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Identity.Services.Impl;

public class ProfilesService : IProfilesService
{
    private readonly IdentityDbContext _dbContext;
    private readonly IdentityCurrentUserService _currentUserService;
    private readonly IAuthService _authService;
    private readonly IBus _bus;

    public ProfilesService(IdentityDbContext dbContext, IdentityCurrentUserService currentUserService,
        IAuthService authService, IBus bus)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _authService = authService;
        _bus = bus;
    }

    public async Task<CurrentProfileResponse> GetCurrentProfileInfo()
    {
        var profile = await _currentUserService.GetCurrentProfile();
        var resp = new CurrentProfileResponse()
        {
            PhoneNumber = profile.User.PhoneNumber,
            InitialName = "user1123",
            NickName = "user123",
            Role = profile.Role.RoleEnum,
            RoleName = profile.Role.Name
        };
        return resp;
    }

    public async Task<JwtTokenResponse> ChangeRole()
    {
        var profile = await _currentUserService.GetCurrentProfile();
        var currentRole = profile.Role.RoleEnum;
        var destinationProfile = _dbContext.Profiles
            .FirstOrDefault(x => x.Role.RoleEnum != currentRole && x.UserId == profile.UserId);
        if (currentRole == RoleEnum.Passenger)
        {
            if (destinationProfile == null)
            {
                destinationProfile = profile.Clone();
                var driverRole = _dbContext.Roles.Single(w => w.RoleEnum == RoleEnum.Driver);
                var newGuid = Guid.NewGuid();
                destinationProfile.Role = driverRole;
                destinationProfile.Guid = newGuid;
                destinationProfile.User = profile.User;
                await _dbContext.AddAsync(destinationProfile);
                await _dbContext.SaveChangesAsync();
            }
        }

        var accessTokenValue = _authService.GenerateAccessToken(destinationProfile);
        var resp = new JwtTokenResponse(accessTokenValue)
        {
            AccessTokenExpiration = DateTime.Now.Add(AuthOptions.AccessTokenLifeTime)
        };
        return resp;
    }

    public async Task<bool> Delete()
    {
        var currentProfile = await _currentUserService.GetCurrentProfile();
        var profiles = await _dbContext.Profiles.Where(x => x.UserId == currentProfile.UserId).ToArrayAsync();
        var message = new DeleteProfileMessage()
        {
            ProfileGuids = profiles.Select(s => s.Guid).ToArray()
        };
        _dbContext.RemoveRange(profiles);
        _dbContext.Remove(currentProfile.User);
        await _bus.Publish(message);
        return true;
    }
}