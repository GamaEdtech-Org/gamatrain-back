namespace GamaEdtech.Presentation.Api.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

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
        [HttpPost("easter-egg"), Produces(typeof(ApiResponse<PointResponseViewModel>))]
        [Permission(policy: null)]
        public async Task<IActionResult<PointResponseViewModel>> TakePoint([NotNull][FromBody] PointRequestViewModel request)
        {
            try
            {
                var result = await gameService.Value.TakePointsAsync(new TakePointsRequestDto
                {
                    Id = request.Id.GetValueOrDefault(),
                    UserId = User.UserId(),
                });

                return Ok<PointResponseViewModel>(new(result.Errors)
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
                return Ok<PointResponseViewModel>(new(new Error { Message = exc.Message }));
            }
        }

        [HttpGet("coins"), Produces(typeof(ApiResponse<CoinsResponseViewModel>))]
        [ApiKey]
        public async Task<IActionResult<CoinsResponseViewModel>> GenerateCoins()
        {
            try
            {
                var result = await gameService.Value.GenerateCoinsAsync();

                return Ok<CoinsResponseViewModel>(new(result.Errors)
                {
                    Data = new()
                    {
                        Coins = result.Data?.Select(t => new CoinViewModel
                        {
                            Id = t.Id,
                            CoinType = t.CoinType,
                            ExpirationTime = t.ExpirationTime,
                        }),
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return Ok<CoinsResponseViewModel>(new(new Error { Message = exc.Message }));
            }
        }
    }
}
