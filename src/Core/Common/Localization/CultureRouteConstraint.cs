namespace GamaEdtech.Common.Localization
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using GamaEdtech.Common.Core;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class CultureRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, [NotNull] RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(Constants.LanguageIdentifier))
            {
                return false;
            }

            var lang = values[Constants.LanguageIdentifier]?.ToString();

            return CultureExtensions.AtomicValues.Any(t => t.Equals(lang, StringComparison.OrdinalIgnoreCase));
        }
    }
}
