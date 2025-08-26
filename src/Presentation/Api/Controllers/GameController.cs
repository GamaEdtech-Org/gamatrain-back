namespace GamaEdtech.Presentation.Api.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Data.Dto.Game;
    using GamaEdtech.Presentation.ViewModel.Game;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    [Route("api/v{version:apiVersion}/[controller]")]
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
                var result = await gameService.Value.TakePointsAsync(new TakePointsDto
                {
                    Points = request.Points,
                });

                if (result.OperationResult != OperationResult.Succeeded)
                {
                    return BadRequest(new ApiResponse<GameResponseViewModel>(result.Errors));
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



        [HttpGet("coins"), Produces(typeof(ApiResponse<GameCoinResponseViewModel>))]
        public async Task<IActionResult> GetCoins()
        {
            try
            {
                var coins = await gameService.Value.GenerateCoinsAsync();

                var response = new GameCoinResponseViewModel
                {
                    Coins = coins
                };

                return Ok(new ApiResponse<GameCoinResponseViewModel> { Data = response });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return Ok<GameCoinResponseViewModel>(new(new Error { Message = exc.Message }));
            }
        }
    }
}

