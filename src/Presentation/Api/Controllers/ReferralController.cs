namespace GamaEdtech.Presentation.Api.Controllers
{
    using Asp.Versioning;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Presentation.ViewModel.Referral;

    using Microsoft.AspNetCore.Mvc;


    //public class ReferralController(Lazy<ILogger<ReferralController>> logger, Lazy<IReferralService> referralService
    //, Lazy<UserManager<ApplicationUser>> userManager, Lazy<IHttpClientFactory> httpClientFactory) : ApiControllerBase<ReferralController>(logger)

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ReferralController(Lazy<ILogger<ReferralController>> logger, Lazy<IHttpContextAccessor> httpContextAccessor
    ) : ApiControllerBase<ReferralController>(logger)
    {

        [HttpPost("generate"), Produces(typeof(ApiResponse<ReferralReponseViewModel>))]
        public IActionResult GenerateRefferalId()
        {
            try
            {
                var userId = httpContextAccessor.Value.HttpContext?.User.UserId();
                if (!userId.HasValue)
                {
                    return Unauthorized();
                }


                var uniqueSourceString = $"{userId}-{Guid.NewGuid()}";

                var bytes = System.Text.Encoding.UTF8.GetBytes(uniqueSourceString);

                var base64String = Convert.ToBase64String(bytes);


                var cleanString = base64String
                                    .Replace("+", string.Empty, StringComparison.Ordinal)
                                    .Replace("/", string.Empty, StringComparison.Ordinal)
                                    .TrimEnd('=');

                var referralCode = cleanString[..10];

                var responseViewModel = new ReferralReponseViewModel
                {
                    ReferralId = referralCode
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
