namespace GamaEdtech.Common.Core
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public abstract class ControllerBase<TClass>(Lazy<ILogger<TClass>> logger) : Controller
        where TClass : class
    {
        protected Lazy<ILogger<TClass>> Logger { get; } = logger;

        [NonAction]
        public RedirectToPageResult RedirectToAreaPage(string? pageName, string? area, object? routeValues = null)
            => RedirectToPage(pageName?.TrimEnd(Constants.PagePostfix) ?? string.Empty, Globals.PrepareValues(routeValues, area));

        [NonAction]
        public RedirectToActionResult RedirectToAreaAction(string? actionName, string? controllerName, string? area, object? routeValues = null)
            => RedirectToAction(actionName, controllerName?.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(routeValues, area));

        [NonAction]
        public override RedirectToActionResult RedirectToAction(string? actionName, string? controllerName, object? routeValues, string? fragment)
            => base.RedirectToAction(actionName, controllerName?.TrimEnd(Constants.ControllerPostfix), Globals.PrepareValues(routeValues), fragment);

        [NonAction]
        public override RedirectToPageResult RedirectToPage(string? pageName, string? pageHandler, object? routeValues, string? fragment)
            => base.RedirectToPage(pageName?.TrimEnd(Constants.PagePostfix) ?? string.Empty, pageHandler, Globals.PrepareValues(routeValues), fragment);
    }
}
