namespace GamaEdtech.Common.Identity.DataProtection
{
    using System;

    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.DataProtection.KeyManagement;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    internal static class DataProtectionExtensions
    {
        public static IDataProtectionBuilder PersistKeysToDbContext(this IDataProtectionBuilder builder, TimeSpan lifetime)
        {
            _ = builder.Services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(services => new ConfigureOptions<KeyManagementOptions>(options =>
                {
                    options.XmlRepository = new CustomXmlRepository(services);
                    options.NewKeyLifetime = lifetime;
                }));

            return builder;
        }
    }
}
