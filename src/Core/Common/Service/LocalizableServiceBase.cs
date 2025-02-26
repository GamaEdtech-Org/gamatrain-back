namespace GamaEdtech.Common.Service
{
    using System;

    using GamaEdtech.Common.DataAccess.UnitOfWork;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    public abstract class LocalizableServiceBase<T>(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<T>> localizer, Lazy<ILogger<T>> logger)
        where T : class
    {
        protected Lazy<ILogger<T>> Logger { get; } = logger;

        protected Lazy<IHttpContextAccessor> HttpContextAccessor { get; } = httpContextAccessor;

        protected Lazy<IStringLocalizer<T>> Localizer { get; } = localizer;

        protected Lazy<IUnitOfWorkProvider> UnitOfWorkProvider { get; } = unitOfWorkProvider;
    }
}
