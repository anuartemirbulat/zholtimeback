using DataContracts.Identity.Requests;
using Identity.DAL;
using Identity.DAL.Entities.Entities;
using Identity.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Identity.Services.Impl;

public class MockRegistrationService : RegistrationService
{
    private readonly IdentityDbContext _identityDbContext;
    private readonly IMessagingService _messagingService;

    public MockRegistrationService(IdentityDbContext identityDbContext, IMessagingService messagingService) : base(
        identityDbContext, messagingService)
    {
        _identityDbContext = identityDbContext;
        _messagingService = messagingService;
    }

    protected override async Task<AccountConfirmation> GetPhoneNumEntity(PhoneConfirmationRequest request)
    {
        var phoneNumEntity = await _identityDbContext.AccountConfirmations
            .OrderByDescending(x => x.DateCreate)
            .FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
        Console.WriteLine($"Типа проверил код: {phoneNumEntity.PhoneNumber}");
        return phoneNumEntity;
    }
}