namespace GamaEdtech.Common.Identity
{
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class ApiDataProtectorTokenProvider<TUser>(IDataProtectionProvider dataProtectionProvider, IOptions<ApiDataProtectorTokenProviderOptions> options, ILogger<ApiDataProtectorTokenProvider<TUser>> logger)
        : DataProtectorTokenProvider<TUser>(dataProtectionProvider, options, logger), IApiDataProtectorTokenProvider<TUser>
        where TUser : class
    {
    }
}
