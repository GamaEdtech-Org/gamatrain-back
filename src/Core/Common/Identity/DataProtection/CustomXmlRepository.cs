namespace GamaEdtech.Common.Identity.DataProtection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Xml.Linq;

    using Microsoft.AspNetCore.DataProtection.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public class CustomXmlRepository(IServiceProvider services) : IXmlRepository
    {
        private readonly IServiceProvider services = services;

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            using var scope = services.CreateScope();
            var dataProtectionService = scope.ServiceProvider.GetRequiredService<IDataProtectionService>();
            return dataProtectionService.GetDataProtections()?.ToArray() ?? [];
        }

        public void StoreElement([NotNull] XElement element, string friendlyName)
        {
            using var scope = services.CreateScope();
            var dataProtectionService = scope.ServiceProvider.GetRequiredService<IDataProtectionService>();
            dataProtectionService.AddDataProtection(new DataProtectionKey
            {
                FriendlyName = friendlyName,
                Xml = element.ToString(SaveOptions.DisableFormatting),
            });
        }
    }
}
