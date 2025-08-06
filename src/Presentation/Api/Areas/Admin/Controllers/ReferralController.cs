namespace GamaEdtech.Presentation.Api.Areas.Admin.Controllers
{
    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Domain.Enumeration;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    [Common.DataAnnotation.Area(nameof(Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    [Permission(Roles = [nameof(Role.Admin)])]
    public class ReferralController(Lazy<ILogger<ReferralController>> logger, Lazy<IReferralService> referralService)
        : ApiControllerBase<LocationsController>(logger)
    {

    }
}
