namespace GamaEdtech.Presentation.Api.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAccess.Specification.Impl;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Data.Dto.Identity;
    using GamaEdtech.Domain.Entity.Identity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Presentation.ViewModel.Identity;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    using Void = Common.Data.Void;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class IdentitiesController(Lazy<ILogger<IdentitiesController>> logger, Lazy<IIdentityService> identityService)
        : ApiControllerBase<IdentitiesController>(logger)
    {
        [HttpPost("login"), Produces(typeof(ApiResponse<AuthenticationResponseViewModel>))]
        [AllowAnonymous]
        public async Task<IActionResult<AuthenticationResponseViewModel>> Login([NotNull] AuthenticationRequestViewModel request)
        {
            try
            {
                var authenticateResult = await identityService.Value.AuthenticateAsync(new AuthenticationRequestDto
                {
                    Username = request.Username!,
                    Password = request.Password!,
                    AuthenticationProvider = AuthenticationProvider.Local,
                });
                if (authenticateResult.Data?.User is null)
                {
                    return Ok<AuthenticationResponseViewModel>(new(authenticateResult.Errors));
                }

                var signInResult = await identityService.Value.SignInAsync(new SignInRequestDto { RememberMe = request.RememberMe, User = authenticateResult.Data.User });
                return Ok<AuthenticationResponseViewModel>(new(signInResult.Errors)
                {
                    Data = signInResult.OperationResult is OperationResult.Succeeded ?
                    new() { Roles = signInResult.Data?.Roles?.ListToFlagsEnum<Role>(), }
                    : null,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<AuthenticationResponseViewModel>(new(new Error { Message = exc.Message }));
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

                return Ok<Void>(new(result.Errors));
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<Void>(new(new Error { Message = exc.Message }));
            }
        }

        [HttpGet("logout"), Produces(typeof(ApiResponse<Void>))]
        [Permission(policy: null)]
        public async Task<IActionResult<Void>> Logout()
        {
            try
            {
                var result = await identityService.Value.SignOutAsync();

                return Ok<Void>(new(result.Errors)
                {
                    Data = result.Data,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<Void>(new(new Error { Message = exc.Message }));
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
                return Ok<Void>(new(result.Errors));
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<Void>(new(new Error { Message = exc.Message }));
            }
        }

        [HttpPost("tokens"), Produces(typeof(ApiResponse<GenerateTokenResponseViewModel>))]
        public async Task<IActionResult<GenerateTokenResponseViewModel>> GenerateToken([NotNull] GenerateTokenRequestViewModel request)
        {
            try
            {
                var authenticateResult = await identityService.Value.AuthenticateAsync(new AuthenticationRequestDto
                {
                    Username = request.Username!,
                    Password = request.Password!,
                    AuthenticationProvider = AuthenticationProvider.Local,
                });
                if (authenticateResult.Data?.User is null)
                {
                    return Ok<GenerateTokenResponseViewModel>(new(authenticateResult.Errors));
                }

                var result = await identityService.Value.GenerateUserTokenAsync(new GenerateUserTokenRequestDto
                {
                    UserId = authenticateResult.Data.User.Id,
                    TokenProvider = PermissionConstants.ApiDataProtectorTokenProvider,
                    Purpose = PermissionConstants.ApiDataProtectorTokenProviderAccessToken,
                });
                return Ok<GenerateTokenResponseViewModel>(new(result.Errors)
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

                return Ok<GenerateTokenResponseViewModel>(new(new Error { Message = exc.Message }));
            }
        }

        /// <summary>
        /// this is temporary, must delete
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("tokens/old"), Produces(typeof(ApiResponse<GenerateTokenResponseViewModel>))]
        public async Task<IActionResult<GenerateTokenResponseViewModel>> GenerateTokenWithOld([NotNull, FromBody] GenerateTokenWithOldRequestViewModel request)
        {
            try
            {
                var result = await identityService.Value.GenerateTokenByCoreTokenAsync(new()
                {
                    Token = request.Token,
                });

                return Ok<GenerateTokenResponseViewModel>(new(result.Errors)
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

                return Ok<GenerateTokenResponseViewModel>(new(new Error { Message = exc.Message }));
            }
        }

        [HttpPost("tokens/google"), Produces(typeof(ApiResponse<GenerateTokenResponseViewModel>))]
        public async Task<IActionResult<GenerateTokenResponseViewModel>> GenerateTokenWithGoogle([NotNull] GenerateTokenWithGoogleRequestViewModel request)
        {
            try
            {
                var authenticateResult = await identityService.Value.AuthenticateAsync(new AuthenticationRequestDto
                {
                    Username = request.Code!,
                    AuthenticationProvider = AuthenticationProvider.Google,
                });
                if (authenticateResult.Data?.User is null)
                {
                    return Ok<GenerateTokenResponseViewModel>(new(authenticateResult.Errors));
                }

                var result = await identityService.Value.GenerateUserTokenAsync(new GenerateUserTokenRequestDto
                {
                    UserId = authenticateResult.Data.User.Id,
                    TokenProvider = PermissionConstants.ApiDataProtectorTokenProvider,
                    Purpose = PermissionConstants.ApiDataProtectorTokenProviderAccessToken,
                });
                return Ok<GenerateTokenResponseViewModel>(new(result.Errors)
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

                return Ok<GenerateTokenResponseViewModel>(new(new Error { Message = exc.Message }));
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
                    UserId = User.UserId(),
                    TokenProvider = PermissionConstants.ApiDataProtectorTokenProvider,
                    Purpose = PermissionConstants.ApiDataProtectorTokenProviderAccessToken,
                });

                return Ok<RevokeTokenResponseViewModel>(new(result.Errors)
                {
                    Data = new()
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<RevokeTokenResponseViewModel>(new(new Error { Message = exc.Message }));
            }
        }

        [HttpGet("authenticated"), Produces(typeof(ApiResponse<bool>))]
        public IActionResult<bool> Authenticated()
        {
            try
            {
                return Ok<bool>(new()
                {
                    Data = User.Identity?.IsAuthenticated is true,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<bool>(new(new Error { Message = exc.Message }));
            }
        }

        [HttpGet("profiles"), Produces(typeof(ApiResponse<ProfileSettingsResponseViewModel>))]
        [Permission(policy: null)]
        public async Task<IActionResult<ProfileSettingsResponseViewModel>> GetProfileSettings()
        {
            try
            {
                var result = await identityService.Value.GetProfileSettingsAsync(new IdEqualsSpecification<ApplicationUser, int>(User.UserId()));

                return Ok<ProfileSettingsResponseViewModel>(new(result.Errors)
                {
                    Data = result.Data is null ? null : new()
                    {
                        UserName = result.Data.UserName,
                        FirstName = result.Data.FirstName,
                        LastName = result.Data.LastName,
                        CountryId = result.Data.CountryId,
                        StateId = result.Data.StateId,
                        CityId = result.Data.CityId,
                        SchoolId = result.Data.SchoolId,
                        ReferralId = result.Data.ReferralId,
                        Gender = result.Data.Gender?.Name,
                        Grade = result.Data.Grade,
                        Board = result.Data.Board,
                        Avatar = result.Data.Avatar,
                        Group = result.Data.Group,
                        CoreId = result.Data.CoreId,
                        WalletId = result.Data.WalletId,
                        ProfileUpdated = result.Data.ProfileUpdated,
                        Roles = result.Data.Roles,
                    },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<ProfileSettingsResponseViewModel>(new(new Error { Message = exc.Message }));
            }
        }

        [HttpPut("profiles"), Produces(typeof(ApiResponse<bool>))]
        [Permission(policy: null)]
        public async Task<IActionResult> UpdateProfileSettings([NotNull] ProfileSettingsRequestViewModel request)
        {
            try
            {
                var result = await identityService.Value.ManageProfileSettingsAsync(new()
                {
                    CityId = request.CityId,
                    SchoolId = request.SchoolId,
                    UserId = User.UserId(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Board = request.Board,
                    Grade = request.Grade,
                    Gender = request.Gender,
                    Group = request.Group,
                    WalletId = request.WalletId,
                    Avatar = await request.Avatar.ConvertImageToBase64Async(),
                });

                return Ok<bool>(new(result.Errors)
                {
                    Data = result.Data,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<bool>(new(new Error { Message = exc.Message }));
            }
        }

        [HttpGet("leader-board"), Produces(typeof(ApiResponse<IEnumerable<UserPointsViewModel>>))]
        public async Task<IActionResult> GetTop100Users([FromQuery] Top100UsersRequestViewModel? request)
        {
            try
            {
                var result = await identityService.Value.GetTop100UsersAsync(new()
                {
                    Board = request?.Board,
                    Grade = request?.Grade,
                    CountryId = request?.CountryId,
                    StateId = request?.StateId,
                    CityId = request?.CityId,
                    SchoolId = request?.SchoolId,
                    RegistrationDateStart = request?.RegistrationDateStart,
                    RegistrationDateEnd = request?.RegistrationDateEnd,
                });

                return Ok<IEnumerable<UserPointsViewModel>>(new(result.Errors)
                {
                    Data = result.Data?.Select(t => new UserPointsViewModel
                    {
                        Name = t.Name,
                        UserId = t.UserId,
                        Points = t.Points,
                        Avatar = t.Avatar,
                    }),
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<IEnumerable<UserPointsViewModel>>(new(new Error { Message = exc.Message }));
            }
        }
    }
}
