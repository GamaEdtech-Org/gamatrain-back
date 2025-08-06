namespace GamaEdtech.Presentation.Api.Areas.Admin.Controllers
{


    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Presentation.ViewModel.Referral;

    using Microsoft.AspNetCore.Mvc;



    [Common.DataAnnotation.Area(nameof(Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    [Permission(Roles = [nameof(Role.Admin)])]
    public class ReferralController(Lazy<ILogger<ReferralController>> logger, Lazy<IReferralService> referralService)
        : ApiControllerBase<ReferralController>(logger)
    {

        [HttpGet("users"), Produces(typeof(ApiResponse<ListDataSource<UsersReferralReponseViewModel>>))]
        public async Task<IActionResult<ListDataSource<UsersReferralReponseViewModel>>> GetAllUsersReferral()
        {
            try
            {
                var result = await referralService.Value.GetAllUsersReferralAsync();

                return Ok(new ApiResponse<ListDataSource<UsersReferralReponseViewModel>>
                {
                    Errors = result.Errors,
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(r => new UsersReferralReponseViewModel
                        {
                            Name = r.Name,
                            Family = r.Family,
                            ReferralId = r.ReferralId
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Value.LogException(ex);
                return Ok<ListDataSource<UsersReferralReponseViewModel>>(new(new Error { Message = ex.Message }));
            }
        }


    }
}
