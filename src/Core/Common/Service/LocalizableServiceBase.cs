namespace GamaEdtech.Common.Service
{
    using System;

    using GamaEdtech.Common.DataAccess.UnitOfWork;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    public abstract class LocalizableServiceBase<T>(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<T>> localizer, Lazy<ILogger<T>> logger)
        : ServiceBase<T>(unitOfWorkProvider, httpContextAccessor, logger)
        where T : class
    {
        protected Lazy<IStringLocalizer<T>> Localizer { get; } = localizer;
    }
}
