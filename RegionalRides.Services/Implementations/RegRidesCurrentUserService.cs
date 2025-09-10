using Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RegionalRides.DAL;
using RegionalRides.DAL.Entities.Entities;
using Services.Interfaces;

namespace RegionalRides.Services.Implementations;

public class RegRidesCurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly RegionalRidesContext _dbContext;

    public RegRidesCurrentUserService(IHttpContextAccessor httpContextAccessor, RegionalRidesContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public async Task<Profile> GetCurrentProfile()
    {
        var profileGuid = this.GetProfileGuidFromHttpContext();
        var profile = await _dbContext.Profiles
            .Where(x => x.Guid == profileGuid)
            .FirstOrDefaultAsync();
        return profile;
    }

    public Guid GetProfileGuidFromHttpContext()
    {
        var value = _httpContextAccessor.HttpContext.User.FindFirst(CustomClaims.ProfileGuid)?.Value;
        return string.IsNullOrEmpty(value) ? Guid.Empty : Guid.Parse(value);
    }
}
