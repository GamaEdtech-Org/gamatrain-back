namespace GamaEdtech.Common.Identity
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class PermissionAttribute : AuthorizeAttribute
    {
        public PermissionAttribute(string? policy = PermissionConstants.PermissionPolicy)
        {
            Policy = policy;
            AuthenticationSchemes = $"{IdentityConstants.ApplicationScheme},{PermissionConstants.TokenAuthenticationScheme}";
        }

        public new string[]? Roles { get; set; }
    }
}
