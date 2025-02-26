namespace GamaEdtech.Presentation.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using System;

    public class HomeController(Lazy<ILogger<HomeController>> logger) : Common.Core.ControllerBase<HomeController>(logger)
    {
        public IActionResult Index() => Redirect("/swagger");
    }
}
