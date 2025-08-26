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
    public class ReferralController(Lazy<ILogger<ReferralController>> logger, Lazy<IIdentityService> referralService)
        : ApiControllerBase<ReferralController>(logger)
    {
        [HttpPost("generate"), Produces(typeof(ApiResponse<ReferralReponseViewModel>))]
        [Permission(policy: null)]
        public async Task<IActionResult<ReferralReponseViewModel>> GenerateRefferalId()
        {
            try
            {
                var result = await referralService.Value.GenerateReferralUserAsync();

                return Ok<ReferralReponseViewModel>(new(result.Errors)
                {
                    Data = result.OperationResult is not OperationResult.Succeeded ? new() : new()
                    {
                        ReferralId = result.Data,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return Ok<ReferralReponseViewModel>(new(new Error { Message = exc.Message }));
            }
        }
    }
}
