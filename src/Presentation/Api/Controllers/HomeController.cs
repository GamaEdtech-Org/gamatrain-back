namespace GamaEdtech.Presentation.Api.Controllers
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class HomeController(Lazy<ILogger<HomeController>> logger)
        : Common.Core.ControllerBase<HomeController>(logger)
    {
        public IActionResult Index() => Redirect("/swagger");
    }
}
