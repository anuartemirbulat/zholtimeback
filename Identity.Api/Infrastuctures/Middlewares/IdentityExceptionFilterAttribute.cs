using Constants;
using DataContracts.Base;
using DataContracts.Exceptions;
using Identity.Services.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Identity.Api.Infrastuctures.Middlewares;

public class IdentityExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<IdentityExceptionFilterAttribute> _logger;
    private readonly IdentityCurrentUserService _currentUserService;

    public IdentityExceptionFilterAttribute(ILogger<IdentityExceptionFilterAttribute> logger,
        IdentityCurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public override async Task OnExceptionAsync(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case IdentityException ex:
                context.Result = new JsonResult(ApiResponse.Failed(ex.ApiErrorCode, ex.Message));
                context.HttpContext.Response.StatusCode = 200;
                _logger.LogInformation($"ErrorCode:{ex.ApiErrorCode}, Message:{ex.Message}," +
                                       $"StackTrace:{ex.StackTrace}");
                break;
            case Exception ex:
                context.Result = new JsonResult(ApiResponse.Failed(ApiErrorCode.UnknownError, ex.Message));
                context.HttpContext.Response.StatusCode = 500;
                // var profile = await _currentUserService.GetCurrentProfile();
                // var profileInfo = profile != null
                //     ? $"{profile.User.PhoneNumber} {profile.Nickname} {profile.Guid};"
                //     : string.Empty;
                _logger.LogError(
                    $"Критическая ошибка:" + $"Message:{ex.Message};" + $"StackTrace:{ex.StackTrace}");
                break;
        }

        await base.OnExceptionAsync(context);
    }
}
