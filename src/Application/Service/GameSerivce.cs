namespace GamaEdtech.Application.Service
{
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Data.Dto.Game;
    using GamaEdtech.Data.Dto.Transaction;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;


    public class GameService(
        Lazy<IUnitOfWorkProvider> unitOfWorkProvider,
        Lazy<IHttpContextAccessor> httpContextAccessor,
        Lazy<IStringLocalizer<GameService>> localizer,
        Lazy<ILogger<GameService>> logger,
        Lazy<ITransactionService> transactionService)
        : LocalizableServiceBase<GameService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IGameService
    {

        public async Task<ResultData<int>> TakePointsAsync([NotNull] TakePointsDto requestDto)
        {
            var userId = HttpContextAccessor.Value.HttpContext?.User.UserId();

            if (!userId.HasValue)
            {
                return new(OperationResult.Failed)
                {
                    Errors = new[] { new Error { Message = Localizer.Value["AuthenticationError"].Value } },
                };
            }

            var transactionRequest = new CreateTransactionRequestDto
            {
                UserId = userId.Value,
                Points = requestDto.Points,
                Description = "the Easter Egg game."
            };

            var result = await transactionService.Value.IncreaseBalanceAsync(transactionRequest);

            if (result.OperationResult == OperationResult.Succeeded)
            {
                return new(OperationResult.Succeeded) { Data = requestDto.Points };
            }

            return new(OperationResult.Failed) { Errors = result.Errors };
        }

        public IReadOnlyList<string> GenerateCoins()
        {
            int target = RandomNumberGenerator.GetInt32(0, 4);

            var coins = new List<string>();
            for (int i = 0; i < target; i++)
            {
                int roll = RandomNumberGenerator.GetInt32(1, 11);

                string reward = roll switch
                {
                    <= 6 => "Bronze",
                    <= 9 => "Silver",
                    _ => "Gold"
                };

                coins.Add(reward);
            }

            return coins.AsReadOnly();
        }
    }
}
