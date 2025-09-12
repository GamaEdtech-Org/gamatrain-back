namespace GamaEdtech.Application.Service
{
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Caching;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Data.Dto.Game;
    using GamaEdtech.Data.Dto.Transaction;
    using GamaEdtech.Domain.Enumeration;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    public class GameService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor,
        Lazy<IStringLocalizer<GameService>> localizer, Lazy<ILogger<GameService>> logger, Lazy<ITransactionService> transactionService
        , Lazy<ICacheProvider> cacheProvider)
        : LocalizableServiceBase<GameService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IGameService
    {
        private const string Prefix = "COIN_";

        public async Task<ResultData<IEnumerable<CoinDto>>> EasterEggFortuneWheelAsync()
        {
            try
            {
                var maxGeneratedCoin = RandomNumberGenerator.GetInt32(3);
                if (maxGeneratedCoin == 0)
                {
                    return new(OperationResult.Succeeded);
                }

                var coins = new List<CoinDto>(maxGeneratedCoin);
                for (var i = 0; i < maxGeneratedCoin; i++)
                {
                    var roll = RandomNumberGenerator.GetInt32(1, 11);

                    var coinType = roll switch
                    {
                        <= 6 => CoinType.Bronze,
                        <= 9 => CoinType.Silver,
                        _ => CoinType.Gold,
                    };

                    var coin = new CoinDto
                    {
                        CoinType = coinType,
                        Id = Guid.NewGuid(),
                        ExpirationTime = DateTimeOffset.UtcNow.AddMinutes(10),
                    };
                    coins.Add(coin);
                    await cacheProvider.Value.SetAsync($"{Prefix}{coin.Id}", coin.CoinType, new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = coin.ExpirationTime,
                    });
                }

                return new(OperationResult.Succeeded) { Data = coins };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<long>> EasterEggPointsAsync([NotNull] EasterEggPointsRequestDto requestDto)
        {
            try
            {
                var key = $"{Prefix}{requestDto.Id}";
                var coin = await cacheProvider.Value.GetAsync<CoinType>(key);
                if (coin is null)
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = "Id is Invalid or has been Expired" },] };
                }

                await cacheProvider.Value.RemoveAsync(key);
                var transactionRequest = new CreateTransactionRequestDto
                {
                    UserId = requestDto.UserId,
                    Points = EnumerationExtensions.ToEnumeration<CoinType, byte>(coin.Name).Points,
                    Description = "the Easter Egg game.",
                };
                var result = await transactionService.Value.IncreaseBalanceAsync(transactionRequest);

                return new(result.OperationResult)
                {
                    Errors = result.Errors,
                    Data = result.Data > 0 ? transactionRequest.Points : 0,
                };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<bool>> SpendPointsAsync([NotNull] SpendPointsRequestDto requestDto)
        {
            try
            {
                var currentBalance = await transactionService.Value.GetCurrentBalanceAsync(new() { UserId = requestDto.UserId });
                if (currentBalance.OperationResult is not OperationResult.Succeeded)
                {
                    return new(OperationResult.Failed) { Errors = currentBalance.Errors };
                }

                if (currentBalance.Data < requestDto.Points)
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = Localizer.Value["InsufficientBalance"] },] };
                }

                var transactionRequest = new CreateTransactionRequestDto
                {
                    UserId = requestDto.UserId,
                    Points = requestDto.Points,
                    Description = "Spend Game Points",
                };
                var result = await transactionService.Value.DecreaseBalanceAsync(transactionRequest);

                return new(result.OperationResult)
                {
                    Errors = result.Errors,
                    Data = result.Data > 0,
                };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }
    }
}
