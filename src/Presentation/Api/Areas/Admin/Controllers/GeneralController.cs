namespace GamaEdtech.Presentation.Api.Areas.Admin.Controllers
{
    using System;

    using Asp.Versioning;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Domain.Enumeration;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Common.DataAnnotation.Area(nameof(Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    [Permission(Roles = [nameof(Role.Admin)])]
    public class GeneralController(Lazy<ILogger<GeneralController>> logger)
        : ApiControllerBase<GeneralController>(logger)
    {
        [HttpGet("system-claims"), Produces(typeof(ApiResponse<IEnumerable<string>?>))]
        public IActionResult<IEnumerable<string>?> GetSystemClaims()
        {
            try
            {
                var lst = Common.Data.Enumeration.FlagsEnumerationExtensions.GetNames<SystemClaim>();
                return Ok<IEnumerable<string>?>(new()
                {
                    Data = lst,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<IEnumerable<string>?>(new(new Error { Message = exc.Message }));
            }
        }
    }
}
