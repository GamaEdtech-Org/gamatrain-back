namespace GamaEdtech.Common.Localization
{
    using System.Linq;

    using GamaEdtech.Common.Core;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.DependencyInjection;

    public static class LocalizationExtensions
    {
        public static RequestLocalizationOptions RequestLocalizationOptions
        {
            get
            {
                var supportedCultures = CultureExtensions.AtomicValues.Select(Globals.GetCulture).ToArray();
                return new RequestLocalizationOptions
                {
                    DefaultRequestCulture = new RequestCulture(Constants.DefaultLanguageCode),
                    SupportedCultures = supportedCultures,
                    SupportedUICultures = supportedCultures,
                    RequestCultureProviders = [new RouteValueRequestCultureProvider(supportedCultures)],
                };
            }
        }

        public static void ConfigureRequestLocalization(this IServiceCollection services)
        {
            var supportedCultures = CultureExtensions.AtomicValues.Select(Globals.GetCulture).ToArray();

            _ = services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(Constants.DefaultLanguageCode);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders.Insert(0, new RouteValueRequestCultureProvider(supportedCultures));
            });
        }
    }
}
