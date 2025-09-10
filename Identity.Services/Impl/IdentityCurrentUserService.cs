using Constants;
using Identity.DAL;
using Identity.DAL.Entities.Entities;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Identity.Services.Impl;

public class IdentityCurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IdentityDbContext _dbContext;

    public IdentityCurrentUserService(IdentityDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Profile> GetCurrentProfile()
    {
        var profileGuid = this.GetProfileGuidFromHttpContext();
        var profile = await _dbContext.Profiles
            .Where(x => x.Guid == profileGuid)
            .Include(i=>i.User)
            .FirstOrDefaultAsync();
        return profile;
    }

    public Guid GetProfileGuidFromHttpContext()
    {
        var value = _httpContextAccessor.HttpContext.User.FindFirst(CustomClaims.ProfileGuid)?.Value;
        return string.IsNullOrEmpty(value) ? Guid.Empty : Guid.Parse(value);
    }
}
