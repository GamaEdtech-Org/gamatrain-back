namespace GamaEdtech.Common.Identity.ApiKey
{
    using Microsoft.AspNetCore.Authorization;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class ApiKeyAttribute : AuthorizeAttribute
    {
        public ApiKeyAttribute()
        {
            Policy = PermissionConstants.ApiKeyPolicy;
            AuthenticationSchemes = PermissionConstants.ApiKeyAuthenticationScheme;
        }

        public new string[]? Roles { get; set; }
    }
}
