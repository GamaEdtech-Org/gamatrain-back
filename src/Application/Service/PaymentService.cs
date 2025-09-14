namespace GamaEdtech.Application.Service
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Data.Dto.Payment;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Infrastructure.Interface;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    public class PaymentService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<PaymentService>> localizer
        , Lazy<ILogger<PaymentService>> logger, Lazy<IEnumerable<ICryptoCurrencyProvider>> cryptoCurrencyProviders, Lazy<IConfiguration> configuration, Lazy<ITransactionService> transactionService)
        : LocalizableServiceBase<PaymentService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IPaymentService
    {
        public async Task<ResultData<long>> CreatePaymentAsync([NotNull] CreatePaymentRequestDto requestDto)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<Payment, long>();

                var payment = new Payment
                {
                    Amount = requestDto.Amount,
                    Currency = requestDto.Currency,
                    UserId = requestDto.UserId,
                    CreationDate = DateTimeOffset.UtcNow,
                    Status = PaymentStatus.Pending,
                };
                repository.Add(payment);
                _ = await uow.SaveChangesAsync();

                return new(OperationResult.Succeeded) { Data = payment.Id };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<bool>> VerifyPaymentAsync([NotNull] VerifyPaymentRequestDto requestDto)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<Payment, long>();

                var payment = await repository.GetManyQueryable(t => t.Id == requestDto.Id).Select(t => new
                {
                    t.Status,
                    t.Currency,
                    t.Amount,
                }).FirstOrDefaultAsync();
                if (payment is null)
                {
                    return new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["PaymentNotFound"] },],
                    };
                }

                if (payment.Status != PaymentStatus.Pending)
                {
                    return new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["InvalidPaymentStatus"] },],
                    };
                }

                if (payment.Currency != requestDto.Currency)
                {
                    return new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["CurrencyMissMatch"] },],
                    };
                }

                _ = configuration.Value.GetValue<string?>("CryptoCurrency:Type").TryGetFromNameOrValue<CryptoCurrencyProviderType, byte>(out var cryptoCurrencyProviderType);
                var provider = cryptoCurrencyProviders.Value.FirstOrDefault(t => t.ProviderType == cryptoCurrencyProviderType);
                var details = await provider!.GetTransactionDetailsAsync(new() { TransactionId = requestDto.TransactionId });
                if (details.OperationResult is not OperationResult.Succeeded)
                {
                    return new(details.OperationResult) { Errors = details.Errors };
                }

                if (details.Data!.Memo != requestDto.Id.ToString())
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = Localizer.Value["PaymentMissMatch"], }] };
                }

                var wallet = configuration.Value.GetValue<string>("CryptoCurrency:Wallet");
                if (details.Data.DestinationWallet != wallet)
                {
                    var message = Localizer.Value["DestinationWalletMissMatch"].Value;
                    _ = await repository.GetManyQueryable(t => t.Id == requestDto.Id).ExecuteUpdateAsync(t => t
                        .SetProperty(p => p.Status, PaymentStatus.Failed)
                        .SetProperty(p => p.TransactionId, requestDto.TransactionId)
                        .SetProperty(p => p.SourceWallet, details.Data.SourceWallet)
                        .SetProperty(p => p.Comment, message)
                        .SetProperty(p => p.VerifyDate, DateTimeOffset.UtcNow));

                    return new(OperationResult.Failed) { Errors = [new() { Message = message, }] };
                }

                if (details.Data.Currency != requestDto.Currency)
                {
                    var message = Localizer.Value["CurrencyMissMatch"].Value;
                    _ = await repository.GetManyQueryable(t => t.Id == requestDto.Id).ExecuteUpdateAsync(t => t
                        .SetProperty(p => p.Status, PaymentStatus.Failed)
                        .SetProperty(p => p.TransactionId, requestDto.TransactionId)
                        .SetProperty(p => p.SourceWallet, details.Data.SourceWallet)
                        .SetProperty(p => p.Comment, message)
                        .SetProperty(p => p.VerifyDate, DateTimeOffset.UtcNow));

                    return new(OperationResult.Failed) { Errors = [new() { Message = message, }] };
                }

                if (details.Data.Amount.GetValueOrDefault() < payment.Amount)
                {
                    var message = Localizer.Value["AmountMissMatch"].Value;
                    _ = await repository.GetManyQueryable(t => t.Id == requestDto.Id).ExecuteUpdateAsync(t => t
                        .SetProperty(p => p.Status, PaymentStatus.Failed)
                        .SetProperty(p => p.TransactionId, requestDto.TransactionId)
                        .SetProperty(p => p.SourceWallet, details.Data.SourceWallet)
                        .SetProperty(p => p.Comment, message)
                        .SetProperty(p => p.VerifyDate, DateTimeOffset.UtcNow));

                    return new(OperationResult.Failed) { Errors = [new() { Message = message, }] };
                }

                var increaseBalance = new Data.Dto.Transaction.CreateTransactionRequestDto
                {
                    UserId = requestDto.UserId,
                    Description = "Payment",
                    IdentifierId = requestDto.Id,
                };
                if (details.Data.Currency == Currency.GET)
                {
                    increaseBalance.Points = (long)(payment.Amount * 2);
                }
                else
                {
                    var res = await provider.ConvertAsync(new()
                    {
                        Amount = payment.Amount,
                        SourceMint = details.Data.Mint,
                        DestinationMint = configuration.Value.GetValue<string?>("CryptoCurrency:Mint"),
                    });
                    if (res.OperationResult is not OperationResult.Succeeded)
                    {
                        return new(res.OperationResult) { Errors = res.Errors, };
                    }

                    if (res.Data is null)
                    {
                        return new(res.OperationResult) { Errors = [new() { Message = Localizer.Value["ConvertError"], }], };
                    }

                    increaseBalance.Points = (long)res.Data.Amount;
                }

                using var trn = uow.CreateTransactionScope();

                _ = await repository.GetManyQueryable(t => t.Id == requestDto.Id).ExecuteUpdateAsync(t => t
                    .SetProperty(p => p.Status, PaymentStatus.Paid)
                    .SetProperty(p => p.TransactionId, requestDto.TransactionId)
                    .SetProperty(p => p.SourceWallet, details.Data.SourceWallet)
                    .SetProperty(p => p.VerifyDate, DateTimeOffset.UtcNow));

                _ = await transactionService.Value.IncreaseBalanceAsync(increaseBalance);

                trn.Complete();

                return new(OperationResult.Succeeded)
                {
                    Data = true,
                };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }
    }
}
