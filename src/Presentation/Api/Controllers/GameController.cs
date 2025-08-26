namespace GamaEdtech.Presentation.Api.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Common.Identity.ApiKey;
    using GamaEdtech.Data.Dto.Game;
    using GamaEdtech.Presentation.ViewModel.Game;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class GameController(Lazy<ILogger<GameController>> logger, Lazy<IGameService> gameService)
        : ApiControllerBase<GameController>(logger)
    {
        [HttpPost("easter-egg"), Produces(typeof(ApiResponse<GameResponseViewModel>))]
        [Permission(policy: null)]
        public async Task<IActionResult<GameResponseViewModel>> TakePoints([NotNull][FromBody] GameRequestViewModel request)
        {
            try
            {
                var result = await gameService.Value.TakePointsAsync(new TakePointsRequestDto
                {
                    Points = request.Points.GetValueOrDefault(),
                    UserId = User.UserId(),
                });

                return Ok<GameResponseViewModel>(new(result.Errors)
                {
                    Data = result.OperationResult is not OperationResult.Succeeded ? new() : new()
                    {
                        Points = result.Data,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return Ok<GameResponseViewModel>(new(new Error { Message = exc.Message }));
            }
        }

        [HttpGet("coins"), Produces(typeof(ApiResponse<GameCoinResponseViewModel>))]
        [ApiKey]
        public IActionResult<GameCoinResponseViewModel> GetCoins()
        {
            try
            {
                var result = gameService.Value.GenerateCoins();

                return Ok<GameCoinResponseViewModel>(new(result.Errors)
                {
                    Data = new()
                    {
                        Coins = result.Data,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return Ok<GameCoinResponseViewModel>(new(new Error { Message = exc.Message }));
            }
        }
    }
}
