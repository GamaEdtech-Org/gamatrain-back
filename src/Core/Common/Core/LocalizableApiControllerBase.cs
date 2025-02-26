namespace GamaEdtech.Common.Core
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("{culture=fa}/api/{area:slugify:exists}/{controller:slugify=Home}/{action:slugify=Index}/{id?}")]
    public abstract class LocalizableApiControllerBase<TClass>(Lazy<ILogger<TClass>> logger, Lazy<IStringLocalizer<TClass>> localizer) : ApiControllerBase<TClass>(logger)
        where TClass : class
    {
        protected Lazy<IStringLocalizer<TClass>> Localizer { get; } = localizer;
    }
}
