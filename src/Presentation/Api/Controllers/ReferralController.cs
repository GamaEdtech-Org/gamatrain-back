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

                // 1. Create a unique source string using the userId and a new Guid.
                // This ensures the value is unique every time the method is called.
                var uniqueSourceString = $"{userId}-{Guid.NewGuid()}";

                // 2. Convert the string to a byte array.
                var bytes = System.Text.Encoding.UTF8.GetBytes(uniqueSourceString);

                // 3. Convert the byte array to a Base64 string.
                // Base64 naturally uses A-Z, a-z, 0-9, but also includes '+' and '/'.
                var base64String = Convert.ToBase64String(bytes);

                // 4. Clean the string to meet your requirements (letters and numbers only).
                // Remove padding ('=') and special characters ('+', '/').
                var cleanString = base64String
                                    .Replace("+", string.Empty, StringComparison.Ordinal)
                                    .Replace("/", string.Empty, StringComparison.Ordinal)
                                    .TrimEnd('=');

                // 5. Truncate to a desired length (e.g., 8 characters for a friendly referral code).
                var referralCode = cleanString[..8];

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
