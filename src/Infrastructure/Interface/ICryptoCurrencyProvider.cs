namespace GamaEdtech.Infrastructure.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.Service.Factory;
    using GamaEdtech.Data.Dto.Payment;
    using GamaEdtech.Domain.Enumeration;

    [Injectable]
    public interface ICryptoCurrencyProvider : IProvider<CryptoCurrencyProviderType>
    {
        Task<ResultData<PaymentInfoResponseDto>> GetTransactionDetailsAsync([NotNull] PaymentInfoRequestDto requestDto);
        Task<ResultData<ConvertResponseDto>> ConvertAsync([NotNull] ConvertRequestDto requestDto);
    }
}
