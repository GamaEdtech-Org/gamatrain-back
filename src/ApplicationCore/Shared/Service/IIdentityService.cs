namespace GamaEdtech.Backend.Shared.Service
{
    using System.Diagnostics.CodeAnalysis;

    using Farsica.Framework.DataAnnotation;

    using Microsoft.AspNetCore.Authentication.Cookies;

    [Injectable]
    public interface IIdentityService
    {
        Task ValidatePrincipalAsync([NotNull] CookieValidatePrincipalContext context);
    }
}
