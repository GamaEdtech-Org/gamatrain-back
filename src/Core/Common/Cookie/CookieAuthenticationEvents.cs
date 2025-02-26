namespace GamaEdtech.Common.Cookie
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    using GamaEdtech.Common.Core;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Routing;

    public class CookieAuthenticationEvents : Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
    {
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly string? action;
        private readonly string? controller;
        private readonly string? area;
        private readonly string? page;

        public CookieAuthenticationEvents(IUrlHelperFactory urlHelperFactory, string? action = null, string? controller = null, string? area = null, string? page = null)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.action = action;
            this.controller = controller;
            this.area = area;
            this.page = page;

            if (string.IsNullOrEmpty(page) && string.IsNullOrEmpty(action))
            {
                throw new ArgumentException("One of Page/Action parameter is Required");
            }
        }

        public override Task RedirectToLogin([NotNull] RedirectContext<CookieAuthenticationOptions> context)
        {
            var returnUrl = System.Web.HttpUtility.ParseQueryString(new Uri(context.RedirectUri).Query)[context.Options.ReturnUrlParameter];
            var routeValues = new RouteValueDictionary
                        {
                            { Constants.LanguageIdentifier, GetCurrentCulture(returnUrl) },
                            { context.Options.ReturnUrlParameter, returnUrl },
                        };

            var data = new RouteData(routeValues);

            var url = urlHelperFactory.GetUrlHelper(new ActionContext(context.HttpContext, data, new ActionDescriptor()));

            context.RedirectUri = string.IsNullOrEmpty(page)
                ? url?.Action(action, controller?.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(routeValues, area)) ?? string.Empty
                : url.Page(page.TrimEnd(Constants.PagePostfix), Globals.PrepareValues(routeValues, area)) ?? string.Empty;

            return base.RedirectToLogin(context);
        }

        private static string? GetCurrentCulture(string? url) => url?.Split("/", StringSplitOptions.RemoveEmptyEntries)?.FirstOrDefault() ?? Constants.DefaultLanguageCode;
    }
}
