using DataContracts.Base;
using DataContracts.Identity.Requests;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/identity/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRegistrationService _registrationService;

        public AuthenticateController(IAuthService authService, IRegistrationService registrationService)
        {
            _authService = authService;
            _registrationService = registrationService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Authenticate([FromBody] AuthRequest request)
        {
            var resp = await _authService.Authenticate(request);
            return Ok(ApiResponse.Success(resp));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendConfirmationCode([FromBody] SendConfirmationCodeRequest request)
        {
            var resp = await _registrationService.SendConfirmationCode(request);
            return Ok(ApiResponse.Success(resp));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ConfirmCode([FromBody] PhoneConfirmationRequest request)
        {
            var resp = await _registrationService.ConfirmationCode(request);
            if (resp.IsSuccess)
            {
                var jwt = await this._authService.Authenticate(request.PhoneNumber, resp.Guid);
                return Ok(ApiResponse.Success(jwt));
            }

            return Ok(ApiResponse.Success(resp));
        }
    }
}