namespace GamaEdtech.Presentation.Api.Controllers
{
    //using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    //using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    //using GamaEdtech.Domain.Entity.Identity;
    using GamaEdtech.Presentation.ViewModel.Referral;
    //using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    //public class ReferralController(Lazy<ILogger<ReferralController>> logger, Lazy<IReferralService> referralService
    //, Lazy<UserManager<ApplicationUser>> userManager, Lazy<IHttpClientFactory> httpClientFactory) : ApiControllerBase<ReferralController>(logger)

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ReferralController(Lazy<ILogger<ReferralController>> logger) : ApiControllerBase<ReferralController>(logger) 
    {

        [HttpPost("generate"), Produces(typeof(ApiResponse<ReferralReponseViewModel>))]
        public async Task<IActionResult<ReferralReponseViewModel>> GenerateRefferalId()
        {

        }


    }
}
