namespace GamaEdtech.Infrastructure.Provider.Authentication
{
    using System;
    using System.Diagnostics.CodeAnalysis;

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

    using static GamaEdtech.Common.Core.Constants;

    public sealed class LocalAuthenticationProvider(Lazy<ILogger<LocalAuthenticationProvider>> logger
        , Lazy<SignInManager<ApplicationUser>> signInManager, Lazy<IStringLocalizer<LocalAuthenticationProvider>> localizer) : IAuthenticationProvider
    {
        public AuthenticationProvider ProviderType => AuthenticationProvider.Local;

        public async Task<ResultData<AuthenticationResponseDto>> AuthenticateAsync([NotNull] AuthenticationRequestDto requestDto)
        {
            try
            {
                var user = await signInManager.Value.UserManager.FindByNameAsync(requestDto.Username);
                var validationResult = ValidateUser<AuthenticationResponseDto>(user);

                if (validationResult.OperationResult is not OperationResult.Succeeded)
                {
                    return validationResult;
                }

                var signinResult = await signInManager.Value.CheckPasswordSignInAsync(user!, requestDto.Password!, true);
                if (signinResult.Succeeded)
                {
                    return new(OperationResult.Succeeded)
                    {
                        Data = new AuthenticationResponseDto { User = user!.AdaptData<ApplicationUser, ApplicationUserDto>() }
                    };
                }

                var message = signinResult.IsLockedOut ? localizer.Value["UserLockedOut"] : localizer.Value["WrongUsernameOrPassword"];
                return new(OperationResult.NotValid)
                {
                    Errors = new[] { new Error { Message = message } },
                };
            }
            catch (Exception exc)
            {
                logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = new[] { new Error { Message = exc.Message }, } };
            }
        }

        private ResultData<T> ValidateUser<T>(ApplicationUser? user)
        {
            IEnumerable<Error> errors = [];
            if (user is null)
            {
                errors = [new() { Message = localizer.Value["UserNotFound"] }];
            }
            else if (!user.Enabled)
            {
                errors = [new() { Message = localizer.Value["UserNotEnabled"] }];
            }

            return new(user?.Enabled == true ? OperationResult.Succeeded : OperationResult.NotValid)
            {
                Errors = errors,
            };
        }
    }
}
