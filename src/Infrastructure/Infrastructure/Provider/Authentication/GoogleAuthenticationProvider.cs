namespace GamaEdtech.Infrastructure.Provider.Authentication
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Mapping;
    using GamaEdtech.Data.Dto.Identity;
    using GamaEdtech.Domain.Entity.Identity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Infrastructure.Interface;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    using static GamaEdtech.Common.Core.Constants;

    public sealed class GoogleAuthenticationProvider(Lazy<ILogger<GoogleAuthenticationProvider>> logger
        , Lazy<SignInManager<ApplicationUser>> signInManager, Lazy<IStringLocalizer<GoogleAuthenticationProvider>> localizer) : IAuthenticationProvider
    {
        private const string IssuerEndpoint = "https://accounts.google.com";

        public AuthenticationProvider ProviderType => AuthenticationProvider.Google;

        public async Task<ResultData<AuthenticationResponseDto>> AuthenticateAsync([NotNull] AuthenticationRequestDto requestDto)
        {
            try
            {
#pragma warning disable CA5404 // Consider calling ConfigureAwait on the awaited task
                var data = await new JwtSecurityTokenHandler().ValidateTokenAsync(requestDto.Username, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = IssuerEndpoint,
                    RequireExpirationTime = true,
                    ValidateLifetime = false,
                    ValidateActor = false,
                    ValidateIssuerSigningKey = false,
                    ValidateSignatureLast = false,
                    SignatureValidator = (string token, TokenValidationParameters parameters) => new JwtSecurityToken(token),
                    ValidateAudience = false,
                    //ValidAudience = "",
                });
#pragma warning restore CA5404 // Consider calling ConfigureAwait on the awaited task
                if (!data.IsValid)
                {
                    return new(OperationResult.NotValid)
                    {
                        Errors = [new Error { Message = localizer.Value["WrongUsernameOrPassword"] }],
                    };
                }

                _ = data.Claims.TryGetValue(ClaimTypes.Email, out var email);
                _ = data.Claims.TryGetValue(ClaimTypes.GivenName, out var firstName);
                _ = data.Claims.TryGetValue(ClaimTypes.Surname, out var lastName);
                //_ = data.Claims.TryGetValue("picture", out var avatar)

                var user = await signInManager.Value.UserManager.FindByNameAsync(email!.ToString()!);
                if (user is null)
                {
                    user = new ApplicationUser
                    {
                        UserName = email.ToString(),
                        Email = email.ToString(),
                        RegistrationDate = DateTime.UtcNow,
                        Enabled = true,
                        FirstName = firstName?.ToString(),
                        LastName = lastName?.ToString(),
                        //Avatar = avatar?.ToString(),
                    };
                    var identityResult = await signInManager.Value.UserManager.CreateAsync(user);
                    if (!identityResult.Succeeded)
                    {
                        return new(OperationResult.NotValid) { Errors = identityResult.Errors.Select(t => new Error { Message = t.Description, Code = t.Code }) };
                    }
                }

                var validationResult = ValidateUser<AuthenticationResponseDto>(user);
                if (validationResult.OperationResult is not OperationResult.Succeeded)
                {
                    return validationResult;
                }

                var dto = user!.AdaptData<ApplicationUser, ApplicationUserDto>();
                return new(OperationResult.Succeeded)
                {
                    Data = new() { User = dto, }
                };
            }
            catch (Exception exc)
            {
                logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = new[] { new Error { Message = exc.Message }, } };
            }
        }

        private ResultData<T> ValidateUser<T>(ApplicationUser user) => user.Enabled
                ? new(OperationResult.Succeeded)
                : new(OperationResult.NotValid)
                {
                    Errors = [new() { Message = localizer.Value["UserNotEnabled"] }],
                };
    }
}
