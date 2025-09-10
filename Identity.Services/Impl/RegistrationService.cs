using DataContracts.Exceptions;
using DataContracts.Identity.Requests;
using DataContracts.Identity.Response;
using Identity.DAL;
using Identity.DAL.Entities.Entities;
using Identity.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Identity.Services.Impl;

public class RegistrationService : IRegistrationService
{
    private readonly IdentityDbContext _identityDbContext;
    private readonly IMessagingService _messagingService;

    public RegistrationService(IdentityDbContext identityDbContext, IMessagingService messagingService)
    {
        _identityDbContext = identityDbContext;
        _messagingService = messagingService;
    }

    public async Task<bool> Register(RegistrationRequest request)
    {
        var phoneNumber = request.PhoneNumber;
        var user = await _identityDbContext.Users
            .Where(x => x.PhoneNumber == phoneNumber && x.Confirmed)
            .FirstOrDefaultAsync();
        if (user == null)
        {
            user = new User()
            {
                PhoneNumber = phoneNumber,
                Password = request.Password,
            };
            await _identityDbContext.AddAsync(user);
        }

        var role = await _identityDbContext.Roles
            .Where(x => x.RoleEnum == request.RoleEnum)
            .SingleAsync();
        var newProfile = new Profile()
        {
            Nickname = phoneNumber.Substring(phoneNumber.Length - 4),
            User = user,
            Role = role
        };
        await _identityDbContext.AddAsync(newProfile);
        return true;
    }

    public async Task<bool> SendConfirmationCode(SendConfirmationCodeRequest request)
    {
        var phoneNumberConfirmation = await _identityDbContext.AccountConfirmations
            .Where(x => x.PhoneNumber == request.PhoneNumber)
            .OrderByDescending(x => x.DateCreate)
            .FirstOrDefaultAsync();
        if (phoneNumberConfirmation != null)
        {
            if (phoneNumberConfirmation.DateCreate + TimeSpan.FromMinutes(1) > DateTime.Now)
            {
                throw new IdentityException("Невозможно отправить повторно код в течении 1 минуты");
            }
        }

        var random = new Random();
        var code = random.Next(1, 99999).ToString("D5");
        var guid = Guid.NewGuid();
        var accountConfirmation = new AccountConfirmation()
        {
            PhoneNumber = request.PhoneNumber,
            Code = code,
            ExpirationDateTime = DateTime.Now.Add(TimeSpan.FromMinutes(5)),
            Guid = guid
        };
        _identityDbContext.Add(accountConfirmation);
        var result = await _messagingService.Send(request.PhoneNumber, $"ZholtimeKZ {code}");
        return result;
    }

    public async Task<PhoneConfirmationResp> ConfirmationCode(PhoneConfirmationRequest request)
    {
        var phoneNumEntity = await this.GetPhoneNumEntity(request);
        if (phoneNumEntity == null)
            throw new IdentityException("Не верный код");

        if (phoneNumEntity.ExpirationDateTime < DateTime.Now)
            throw new IdentityException("Срок действия кода истек");

        phoneNumEntity.Confirmed = true;
        _identityDbContext.AccountConfirmations.Update(phoneNumEntity);
        var resp = new PhoneConfirmationResp(true, phoneNumEntity.Guid);
        return resp;
    }

    protected virtual async Task<AccountConfirmation> GetPhoneNumEntity(PhoneConfirmationRequest request)
    {
        var accountConfirmation = await _identityDbContext.AccountConfirmations
            .OrderByDescending(x => x.DateCreate)
            .Where(x => x.PhoneNumber == request.PhoneNumber && x.Code == request.Code)
            .FirstOrDefaultAsync();
        return accountConfirmation;
    }
}