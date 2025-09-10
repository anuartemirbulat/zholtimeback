using DataContracts.Identity.Requests;
using DataContracts.Identity.Response;

namespace Identity.Services.Interfaces;

public interface IRegistrationService
{
    Task<bool> SendConfirmationCode(SendConfirmationCodeRequest request);
    Task<PhoneConfirmationResp> ConfirmationCode(PhoneConfirmationRequest request);
}
