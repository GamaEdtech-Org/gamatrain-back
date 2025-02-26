namespace GamaEdtech.Common.Converter
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    public class ConfigureJsonOptions(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider) : IConfigureOptions<JsonOptions>
    {
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly IServiceProvider serviceProvider = serviceProvider;

        public void Configure([NotNull] JsonOptions options)
        {
            options.JsonSerializerOptions.Converters.Add(new ServiceProviderDummyConverter(httpContextAccessor, serviceProvider));
            options.JsonSerializerOptions.TypeInfoResolver = new CustomtJsonTypeInfoResolver();
        }
    }
}
