namespace GamaEdtech.Common.Infrastructure
{
    using System;

    using GamaEdtech.Common.HttpProvider;

    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    public abstract class InfrastructureBase<T>(Lazy<IHttpProvider> httpProvider, Lazy<IStringLocalizer<T>> localizer, Lazy<ILogger<T>> logger)
        where T : class
    {
        protected Lazy<IHttpProvider> HttpProvider { get; } = httpProvider;

        protected Lazy<IStringLocalizer<T>> Localizer { get; } = localizer;

        protected Lazy<ILogger<T>> Logger { get; } = logger;
    }
}
