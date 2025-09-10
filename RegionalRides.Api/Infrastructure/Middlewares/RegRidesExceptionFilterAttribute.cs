using Constants;
using DataContracts.Base;
using DataContracts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RegionalRides.Services.Implementations;

namespace RegionalRides.Api.Infrastructure.Middlewares;

public class RegRidesExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<RegRidesExceptionFilterAttribute> _logger;
    private readonly RegRidesCurrentUserService _currentUserService;

    public RegRidesExceptionFilterAttribute(RegRidesCurrentUserService currentUserService,
        ILogger<RegRidesExceptionFilterAttribute> logger)
    {
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public override async Task OnExceptionAsync(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case RegRidesException ex:
                context.Result = new JsonResult(ApiResponse.Failed(ex.ApiErrorCode, ex.Message));
                context.HttpContext.Response.StatusCode = 200;
                _logger.LogInformation($"ErrorCode:{ex.ApiErrorCode}, Message:{ex.Message}," +
                                       $"StackTrace:{ex.StackTrace}");
                break;
            case Exception ex:
                context.Result = new JsonResult(ApiResponse.Failed(ApiErrorCode.UnknownError, ex.Message));
                context.HttpContext.Response.StatusCode = 500;
                var profile = await _currentUserService.GetCurrentProfile();
                var profileInfo = profile != null
                    ? $"{profile.PhoneNumber} {profile.Nickname} {profile.Guid};"
                    : string.Empty;
                _logger.LogError(
                    $"Критическая ошибка:" + profileInfo
                                           + $"Message:{ex.Message};"
                                           + $"StackTrace:{ex.StackTrace}");
                break;
        }

        await base.OnExceptionAsync(context);
    }
}
