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

    public class GameService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor,
        Lazy<IStringLocalizer<GameService>> localizer, Lazy<ILogger<GameService>> logger, Lazy<ITransactionService> transactionService)
        : LocalizableServiceBase<GameService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IGameService
    {
        public async Task<ResultData<int>> TakePointsAsync([NotNull] TakePointsRequestDto requestDto)
        {
            try
            {
                var transactionRequest = new CreateTransactionRequestDto
                {
                    UserId = requestDto.UserId,
                    Points = requestDto.Points,
                    Description = "the Easter Egg game.",
                };
                var result = await transactionService.Value.IncreaseBalanceAsync(transactionRequest);

                return new(OperationResult.Succeeded)
                {
                    Errors = result.Errors,
                    Data = requestDto.Points,
                };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public ResultData<IEnumerable<string?>> GenerateCoins()
        {
            try
            {
                var length = RandomNumberGenerator.GetInt32(0, 4);

                var coins = new List<string?>();
                for (var i = 0; i < length; i++)
                {
                    var roll = RandomNumberGenerator.GetInt32(1, 11);

                    var reward = roll switch
                    {
                        <= 6 => "Bronze",
                        <= 9 => "Silver",
                        _ => "Gold"
                    };

                    coins.Add(reward);
                }

                return new(OperationResult.Succeeded) { Data = coins };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }
    }
}
