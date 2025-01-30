namespace GamaEdtech.Backend.DomainService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;

    using Farsica.Framework.Core;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.UnitOfWork;
    using Farsica.Framework.Service;

    using GamaEdtech.Backend.Data.Entity.Identity;
    using GamaEdtech.Backend.Shared.Service;

    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static Farsica.Framework.Core.Constants;

    public class IdentityService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<IdentityService>> localizer, Lazy<ILogger<IdentityService>> logger
            , Lazy<UserManager<ApplicationUser>> userManager, Lazy<SignInManager<ApplicationUser>> signInManager)
        : LocalizableServiceBase<IdentityService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IIdentityService
    {
        public async Task ValidatePrincipalAsync([NotNull] CookieValidatePrincipalContext context)
        {
            var systemClaim = context.Principal?.FindFirstValue(ClaimTypes.System);
            if (string.IsNullOrEmpty(systemClaim))
            {
                await handleUnauthorizedRequestAsync();
                return;
            }

            var hash = GenerateDeviceHash(context.HttpContext);
            if (!systemClaim.Equals(hash, StringComparison.OrdinalIgnoreCase))
            {
                await handleUnauthorizedRequestAsync();
            }

            var userId = context.Principal.UserId<int>();
            var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
            var currentSecurityStamp = await uow.GetRepository<ApplicationUser, int>().GetManyQueryable(t => t.Id == userId).Select(t => t.SecurityStamp).FirstOrDefaultAsync();
            var securityStampClaim = context.Principal?.FindFirstValue(userManager.Value.Options.ClaimsIdentity.SecurityStampClaimType);
            if (currentSecurityStamp != securityStampClaim)
            {
                await handleUnauthorizedRequestAsync();
            }

            async Task handleUnauthorizedRequestAsync()
            {
                context.RejectPrincipal();
                _ = await SignOutAsync();
            }
        }

        public async Task<ResultData<Farsica.Framework.Data.Void>> SignOutAsync()
        {
            try
            {
                await signInManager.Value.SignOutAsync();

                return new(OperationResult.Succeeded) { Data = new() };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = new[] { new Error { Message = exc.Message }, } };
            }
        }

        private static string? GenerateDeviceHash(HttpContext? httpContext)
        {
            var ip = httpContext.GetClientIpAddress();
            var userAgent = httpContext.UserAgent();

            var byteValue = Encoding.UTF8.GetBytes(ip + userAgent);
            var byteHash = SHA256.HashData(byteValue);
            return Convert.ToBase64String(byteHash);
        }
    }
}
