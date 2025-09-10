using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Constants;
using Constants.Enums;
using Core;
using DataContracts.Exceptions;
using DataContracts.Identity.Requests;
using DataContracts.Identity.Response;
using DataContracts.Messages;
using Identity.DAL;
using Identity.DAL.Entities.Entities;
using Identity.Services.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Services.Impl;

public class AuthService : IAuthService
{
    private readonly IdentityDbContext _identityDbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IBus _bus;

    public AuthService(IdentityDbContext identityDbContext, IPasswordHasher<User> passwordHasher, IBus bus)
    {
        _identityDbContext = identityDbContext;
        _passwordHasher = passwordHasher;
        _bus = bus;
    }

    public async Task<JwtTokenResponse> Authenticate(AuthRequest request)
    {
        var profile = await _identityDbContext.Profiles
            .Where(x => x.User.PhoneNumber == request.PhoneNumber
                        && x.Role.RoleEnum == request.RoleEnum)
            .FirstOrDefaultAsync();
        if (profile == null)
            throw new IdentityException("Пользователь не найден");
        var verifyResult = _passwordHasher.VerifyHashedPassword(profile.User, profile.User.Password, request.Password);
        if (verifyResult == PasswordVerificationResult.Failed)
            throw new IdentityException("Не верный логин/пароль");
        var accessTokenValue = this.GenerateAccessToken(profile);
        var resp = new JwtTokenResponse(accessTokenValue)
        {
            AccessTokenExpiration = DateTime.Now.Add(AuthOptions.AccessTokenLifeTime)
        };
        return resp;
    }

    public async Task<JwtTokenResponse> Authenticate(string phoneNumber, Guid guid)
    {
        var accountConfigmation = await _identityDbContext.AccountConfirmations
            .Where(x => x.Guid == guid && x.PhoneNumber == phoneNumber)
            .FirstOrDefaultAsync();
        if (accountConfigmation == null)
            throw new IdentityException("Код не верный");

        var profile = await _identityDbContext.Profiles
            .Where(x => x.User.PhoneNumber == accountConfigmation.PhoneNumber)
            .FirstOrDefaultAsync();

        if (profile == null)
        {
            var role = await _identityDbContext.Roles
                .Where(x => x.RoleEnum == RoleEnum.Passenger)
                .SingleAsync();
            var newUser = new User()
            {
                PhoneNumber = phoneNumber,
                Confirmed = true
            };
            _identityDbContext.Add(newUser);
            var newGuid = Guid.NewGuid();
            profile = new Profile()
            {
                User = newUser,
                Role = role,
                Guid = newGuid,
                Nickname = "WaygoUser",
                InitialName = "WaygoUser"
            };
            await _identityDbContext.AddAsync(profile);
            await _bus.Publish(new NewProfileMessage(newGuid, profile.InitialName, profile.Nickname, phoneNumber));
        }

        var accessTokenValue = this.GenerateAccessToken(profile);
        var resp = new JwtTokenResponse(accessTokenValue)
        {
            AccessTokenExpiration = DateTime.Now.Add(AuthOptions.AccessTokenLifeTime)
        };
        return resp;
    }

    public string GenerateAccessToken(Profile profile)
    {
        var signCred = new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
        var now = DateTime.Now;
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: GenerateToken(profile).Claims,
            expires: now.Add(AuthOptions.AccessTokenLifeTime),
            signingCredentials: signCred);
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }

    private ClaimsIdentity GenerateToken(Profile profile)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, profile.User.PhoneNumber),
            new Claim(ClaimsIdentity.DefaultNameClaimType, profile.User.PhoneNumber),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, profile.Role.RoleEnum.ToString()),
            new Claim(CustomClaims.ProfileGuid, profile.Guid.ToString())
        };
        ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }
}
