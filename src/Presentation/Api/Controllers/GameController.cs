namespace GamaEdtech.Presentation.Api.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Data.Dto.Game;
    using GamaEdtech.Presentation.ViewModel.Game;
    using GamaEdtech.Presentation.ViewModel.Referral;

    using Microsoft.AspNetCore.Mvc;

    using static GamaEdtech.Common.Core.Constants;

    [Route("api/[controller]")]
    [ApiController]
    public class GameController(
        Lazy<ILogger<GameController>> logger,
        Lazy<IGameService> gameService
    ) : ApiControllerBase<GameController>(logger)
    {
        [HttpPost("easter-egg"), Produces(typeof(ApiResponse<GameResponseViewModel>))]
        [Permission(policy: null)]
        public async Task<IActionResult> TakePoints([NotNull][FromBody] GameRequestViewModel request)
        {
            try
            {
                if (request.Points < 1 || request.Points > 1000)
                {
                    return BadRequest(new ApiResponse<GameResponseViewModel>(
                        new Error { Message = "Points value must be between 1 and 1000." }
                    ));
                }

                var result = await gameService.Value.TakePointsAsync(new TakePointsDto
                {
                    Points = request.Points,
                });

                if (result.OperationResult != OperationResult.Succeeded)
                {
                    return BadRequest(new ApiResponse<ReferralReponseViewModel>(result.Errors));
                }

                var responseViewModel = new GameResponseViewModel
                {
                    Points = result.Data
                };

                return Ok(new ApiResponse<GameResponseViewModel> { Data = responseViewModel });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return Ok<GameResponseViewModel>(new(new Error { Message = exc.Message }));
            }
        }
    }
}
