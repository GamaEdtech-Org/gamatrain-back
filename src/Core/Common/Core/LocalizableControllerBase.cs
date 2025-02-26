namespace GamaEdtech.Common.Core
{
    using System;

    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    public abstract class LocalizableControllerBase<TClass>(Lazy<ILogger<TClass>> logger, Lazy<IStringLocalizer<TClass>> localizer) : ControllerBase<TClass>(logger)
        where TClass : class
    {
        protected Lazy<IStringLocalizer<TClass>> Localizer { get; } = localizer;
    }
}
