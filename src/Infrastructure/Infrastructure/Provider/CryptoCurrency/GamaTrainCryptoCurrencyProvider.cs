namespace GamaEdtech.Infrastructure.Provider.CryptoCurrency
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.HttpProvider;
    using GamaEdtech.Common.Infrastructure;
    using GamaEdtech.Data.Dto.Payment;
    using GamaEdtech.Data.Dto.Provider.CryptoCurrency;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Infrastructure.Interface;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    public sealed class GamaTrainCryptoCurrencyProvider(Lazy<IConfiguration> configuration, Lazy<IHttpProvider> httpProvider, Lazy<IStringLocalizer<GamaTrainCryptoCurrencyProvider>> localizer
        , Lazy<ILogger<GamaTrainCryptoCurrencyProvider>> logger)
        : InfrastructureBase<GamaTrainCryptoCurrencyProvider>(httpProvider, localizer, logger), ICryptoCurrencyProvider
    {
        public CryptoCurrencyProviderType ProviderType => CryptoCurrencyProviderType.GamaTrain;

        public async Task<ResultData<PaymentInfoResponseDto>> GetTransactionDetailsAsync([NotNull] PaymentInfoRequestDto requestDto)
        {
            try
            {
                var response = await HttpProvider.Value.PostAsync<GamaTrainTransactionDetailsRequest, GamaTrainTransactionDetailsResponse, GamaTrainTransactionDetailsRequest>(new()
                {
                    Uri = configuration.Value.GetValue<string>("CryptoCurrency:GamaTrain:Uri"),
                    Request = new(),
                    Body = new()
                    {
                        ApiKey = configuration.Value.GetValue<string>("CryptoCurrency:GamaTrain:ApiKey"),
                        TransactionId = requestDto.TransactionId,
                    },
                });
                if (response is null)
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = Localizer.Value["GeneralError"], }] };
                }

                if (response.Error)
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = response.ErrorMessage, }] };
                }

                if (response.Transfer is null)
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = Localizer.Value["NullTransferInfo"], }] };
                }

                var data = new PaymentInfoResponseDto
                {
                    Amount = response.Transfer.Amount,
                    Currency = response.Transfer.Currency,
                    DestinationWallet = response.Transfer.DestinationWallet,
                    SourceWallet = response.Transfer.SourceWallet,
                    Mint = response.Transfer.Mint,
                    Memo = response.Memo,
                };
                return new(OperationResult.Succeeded)
                {
                    Data = data,
                };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<ConvertResponseDto>> ConvertAsync([NotNull] ConvertRequestDto requestDto)
        {
            try
            {
                var response = await HttpProvider.Value.GetAsync<GamaTrainConvertRequest, GamaTrainConvertResponse, GamaTrainConvertRequest>(new()
                {
                    Uri = configuration.Value.GetValue<string>("CryptoCurrency:GamaTrain:ConvertUri"),
                    Request = new(),
                    Body = new()
                    {
                        Amount = (long)requestDto.Amount,
                        SourceMint = requestDto.SourceMint,
                        DestinationMint = requestDto.DestinationMint,
                    },
                });
                if (response is null)
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = Localizer.Value["GeneralError"], }] };
                }

                var data = new ConvertResponseDto
                {
                    Amount = response.Amount,
                };
                return new(OperationResult.Succeeded)
                {
                    Data = data,
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
