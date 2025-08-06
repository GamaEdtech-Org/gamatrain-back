namespace GamaEdtech.Presentation.Api.Controllers
{
    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Presentation.ViewModel.Referral;

    using Microsoft.AspNetCore.Mvc;

    using static GamaEdtech.Common.Core.Constants;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ReferralController(
        Lazy<ILogger<ReferralController>> logger,
        Lazy<IHttpContextAccessor> httpContextAccessor,
        Lazy<IReferralService> referralService
    ) : ApiControllerBase<ReferralController>(logger)
    {
        [HttpPost("generate"), Produces(typeof(ApiResponse<ReferralReponseViewModel>))]
        public async Task<IActionResult> GenerateRefferalId()
        {
            try
            {
                var userId = httpContextAccessor.Value.HttpContext?.User.UserId();
                if (!userId.HasValue)
                {
                    return Unauthorized();
                }

                var firstName = httpContextAccessor.Value.HttpContext?.User.FindFirst("FirstName")?.Value;
                var lastName = httpContextAccessor.Value.HttpContext?.User.FindFirst("LastName")?.Value;

                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                {
                    return BadRequest(new ApiResponse<ReferralReponseViewModel>(
                        new Error { Message = "User first name or last name is missing." }));
                }

                var uniqueSourceString = $"{userId}-{Guid.NewGuid()}";
                var bytes = System.Text.Encoding.UTF8.GetBytes(uniqueSourceString);
                var base64String = Convert.ToBase64String(bytes);
                var cleanString = base64String
                    .Replace("+", string.Empty, StringComparison.Ordinal)
                    .Replace("/", string.Empty, StringComparison.Ordinal)
                    .TrimEnd('=');
                var referralCode = cleanString[..10];

                var referralUser = new ReferralUser
                {
                    Name = firstName,
                    Family = lastName,
                    ReferralId = referralCode,
                    CreationUserId = userId.Value,
                    CreationDate = DateTimeOffset.UtcNow
                };

                var result = await referralService.Value.CreateRefrralUserAsync(referralUser);

                if (result.OperationResult != OperationResult.Succeeded)
                {
                    return BadRequest(new ApiResponse<ReferralReponseViewModel>(result.Errors));
                }

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
