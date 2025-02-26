namespace GamaEdtech.Common.Identity
{
    using System;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;

    public class ApiDataProtectorTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public ApiDataProtectorTokenProviderOptions() => Name = PermissionConstants.ApiDataProtectorTokenProvider;

        public static TimeSpan GetTokenLifespan(IConfiguration configuration) => configuration.GetValue<TimeSpan>("IdentityOptions:Tokens:ApiDataProtectorTokenProviderOptions:TokenLifespan");
    }
}
