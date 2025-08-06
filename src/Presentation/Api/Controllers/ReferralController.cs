namespace GamaEdtech.Presentation.Api.Controllers
{
    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Presentation.ViewModel.Referral;

    using Microsoft.AspNetCore.Mvc;

    using static GamaEdtech.Common.Core.Constants;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ReferralController(
        Lazy<ILogger<ReferralController>> logger,
        Lazy<IReferralService> referralService
    ) : ApiControllerBase<ReferralController>(logger)
    {
        [HttpPost("generate"), Produces(typeof(ApiResponse<ReferralReponseViewModel>))]
        [Permission(policy: null)]
        public async Task<IActionResult> GenerateRefferalId()
        {
            try
            {
                var result = await referralService.Value.CreateRefrralUserAsync();

                if (result.OperationResult != OperationResult.Succeeded)
                {
                    return BadRequest(new ApiResponse<ReferralReponseViewModel>(result.Errors));
                }

                var responseViewModel = new ReferralReponseViewModel
                {
                    ReferralId = result.Data ?? string.Empty,
                };

                return Ok(new ApiResponse<ReferralReponseViewModel> { Data = responseViewModel });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return Ok<ReferralReponseViewModel>(new(new Error { Message = exc.Message }));
            }
        }
    }
}
