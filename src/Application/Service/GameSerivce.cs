namespace GamaEdtech.Application.Service
{
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Caching;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
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

        public async Task<ResultData<int>> TakePointsAsync([NotNull] TakePointsRequestDto requestDto)
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
                    Points = coin.Points,
                    Description = "the Easter Egg game.",
                };
                var result = await transactionService.Value.IncreaseBalanceAsync(transactionRequest);

                return new(OperationResult.Succeeded)
                {
                    Errors = result.Errors,
                    Data = result.Data > 0 ? coin.Points : 0,
                };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<IEnumerable<CoinDto>>> GenerateCoinsAsync()
        {
            try
            {
                const int maxGeneratedCoin = 3;
                var coins = new List<CoinDto>(maxGeneratedCoin);
                ReadOnlySpan<CoinType> types = [CoinType.Bronze, CoinType.Silver, CoinType.Gold];
                var randomTypes = RandomNumberGenerator.GetItems(types, maxGeneratedCoin);
                for (var i = 0; i < randomTypes.Length; i++)
                {
                    var coin = new CoinDto
                    {
                        CoinType = randomTypes[i],
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
    }
}
