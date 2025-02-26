namespace GamaEdtech.Presentation.Api.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Data.Dto.Identity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Presentation.ViewModel.Identity;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    using Void = Common.Data.Void;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class IdentitiesController(Lazy<ILogger<IdentitiesController>> logger, Lazy<IIdentityService> identityService) : ApiControllerBase<IdentitiesController>(logger)
    {
        [HttpPost("login"), Produces(typeof(ApiResponse<AuthenticationResponseViewModel>))]
        [AllowAnonymous]
        public async Task<IActionResult<AuthenticationResponseViewModel>> Login([NotNull] AuthenticationRequestViewModel request)
        {
            try
            {
                var authenticateResult = await identityService.Value.AuthenticateAsync(new AuthenticationRequestDto { Password = request.Password!, Username = request.Username! });
                if (authenticateResult.Data?.User is null)
                {
                    return Ok(new ApiResponse<AuthenticationResponseViewModel>
                    {
                        Errors = authenticateResult.Errors,
                    });
                }

                var signInResult = await identityService.Value.SignInAsync(new SignInRequestDto { RememberMe = request.RememberMe, User = authenticateResult.Data.User });
                return Ok(new ApiResponse<AuthenticationResponseViewModel>(signInResult.Errors)
                {
                    Data = signInResult.OperationResult is OperationResult.Succeeded ?
                    new() { Roles = signInResult.Data?.Roles?.ListToFlagsEnum<Role>(), }
                    : null,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<AuthenticationResponseViewModel>(new Error { Message = exc.Message }));
            }
        }

        [HttpPost("register"), Produces(typeof(ApiResponse<Void>))]
        [AllowAnonymous]
        public async Task<IActionResult<Void>> Register([NotNull] RegistrationRequestViewModel request)
        {
            try
            {
                var result = await identityService.Value.RegisterAsync(new()
                {
                    Username = request.Email!,
                    Password = request.Password!,
                    Email = request.Email!,
                });

                return Ok(new ApiResponse<Void>(result.Errors));
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<Void>(new Error { Message = exc.Message }));
            }
        }

        [HttpGet("logout"), Produces(typeof(ApiResponse<Void>))]
        [Permission(policy: null)]
        public async Task<IActionResult<Void>> Logout()
        {
            try
            {
                var result = await identityService.Value.SignOutAsync();

                return Ok(new ApiResponse<Void>(result.Errors)
                {
                    Data = result.Data,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<Void>(new Error { Message = exc.Message }));
            }
        }

        [HttpPut("password"), Produces(typeof(ApiResponse<Void>))]
        [Permission(policy: null)]
        public async Task<IActionResult<Void>> ChangePassword([NotNull] ChangePasswordRequestViewModel request)
        {
            try
            {
                var result = await identityService.Value.ChangePasswordAsync(new ChangePasswordRequestDto
                {
                    CurrentPassword = request.CurrentPassword,
                    NewPassword = request.NewPassword,
                });
                return Ok(new ApiResponse<Void>(result.Errors));
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<Void>(new Error { Message = exc.Message }));
            }
        }

        [HttpPost("tokens"), Produces(typeof(ApiResponse<GenerateTokenResponseViewModel>))]
        public async Task<IActionResult<GenerateTokenResponseViewModel>> GenerateToken([NotNull] GenerateTokenRequestViewModel request)
        {
            try
            {
                var authenticateResult = await identityService.Value.AuthenticateAsync(new AuthenticationRequestDto { Password = request.Password!, Username = request.Username! });
                if (authenticateResult.Data?.User is null)
                {
                    return Ok(new ApiResponse<GenerateTokenResponseViewModel>(authenticateResult.Errors));
                }

                var signInResult = await identityService.Value.SignInAsync(new SignInRequestDto { RememberMe = false, User = authenticateResult.Data.User });
                if (signInResult.OperationResult is not OperationResult.Succeeded)
                {
                    return Ok(new ApiResponse<GenerateTokenResponseViewModel>(signInResult.Errors));
                }

                var result = await identityService.Value.GenerateUserTokenAsync(new GenerateUserTokenRequestDto
                {
                    UserId = authenticateResult.Data.User.Id,
                    TokenProvider = PermissionConstants.ApiDataProtectorTokenProvider,
                    Purpose = PermissionConstants.ApiDataProtectorTokenProviderAccessToken,
                });
                return Ok(new ApiResponse<GenerateTokenResponseViewModel>(result.Errors)
                {
                    Data = new()
                    {
                        Token = result.Data?.Token,
                        ExpirationTime = result.Data?.ExpirationTime,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<GenerateTokenResponseViewModel>(new Error { Message = exc.Message }));
            }
        }

        [HttpPost("tokens/revoke"), Produces(typeof(ApiResponse<RevokeTokenResponseViewModel>))]
        [Permission(policy: null)]
        public async Task<IActionResult<RevokeTokenResponseViewModel>> RevokeToken()
        {
            try
            {
                var result = await identityService.Value.RemoveUserTokenAsync(new RemoveUserTokenRequestDto
                {
                    UserId = User.UserId<int>(),
                    TokenProvider = PermissionConstants.ApiDataProtectorTokenProvider,
                    Purpose = PermissionConstants.ApiDataProtectorTokenProviderAccessToken,
                });

                return Ok(new ApiResponse<RevokeTokenResponseViewModel>(result.Errors)
                {
                    Data = new()
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<RevokeTokenResponseViewModel>(new Error { Message = exc.Message }));
            }
        }

        [HttpGet("authenticated"), Produces(typeof(ApiResponse<bool>))]
        public IActionResult<bool> Authenticated()
        {
            try
            {
                return Ok(new ApiResponse<bool>
                {
                    Data = User.Identity?.IsAuthenticated is true,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<bool>(new Error { Message = exc.Message }));
            }
        }

        [HttpGet("profiles"), Produces(typeof(ApiResponse<ProfileSettingsResponseViewModel>))]
        [Permission(policy: null)]
        public async Task<IActionResult<ProfileSettingsResponseViewModel>> GetProfileSettings()
        {
            try
            {
                var result = await identityService.Value.GetProfileSettingsAsync();

                return Ok(new ApiResponse<ProfileSettingsResponseViewModel>(result.Errors)
                {
                    Data = new ProfileSettingsResponseViewModel
                    {
                        TimeZoneId = result.Data?.TimeZoneId,
                    },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ProfileSettingsResponseViewModel>(new Error { Message = exc.Message }));
            }
        }

        [HttpPut("profiles"), Produces(typeof(ApiResponse<Void>))]
        [Permission(policy: null)]
        public async Task<IActionResult<Void>> UpdateProfileSettings([NotNull] ProfileSettingsRequestViewModel request)
        {
            try
            {
                var result = await identityService.Value.UpdateProfileSettingsAsync(new ProfileSettingsDto
                {
                    TimeZoneId = request.TimeZoneId,
                });

                return Ok(new ApiResponse<Void>(result.Errors));
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<Void>(new Error { Message = exc.Message }));
            }
        }
    }
}
