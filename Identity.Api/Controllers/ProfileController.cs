using DataContracts.Base;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [Route("api/identity/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfilesService _profilesService;

        public ProfileController(IProfilesService profilesService)
        {
            _profilesService = profilesService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCurrentProfileInfo()
        {
            var profile = await _profilesService.GetCurrentProfileInfo();
            return Ok(ApiResponse.Success(profile));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangeRole()
        {
            var jwt = await _profilesService.ChangeRole();
            return Ok(ApiResponse.Success(jwt));
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete()
        {
            var resp = await _profilesService.Delete();
            return Ok(ApiResponse.Success(resp));
        }
    }
}