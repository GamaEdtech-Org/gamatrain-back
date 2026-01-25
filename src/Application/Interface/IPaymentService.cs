namespace GamaEdtech.Application.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Data.Dto.Payment;
    using GamaEdtech.Domain.Entity;

    [Injectable]
    public interface IPaymentService
    {
        Task<ResultData<ListDataSource<PaymentDto>>> GetPaymentsAsync(ListRequestDto<Payment>? requestDto = null);
        Task<ResultData<long>> CreatePaymentAsync([NotNull] CreatePaymentRequestDto requestDto);
        Task<ResultData<bool>> VerifyPaymentAsync([NotNull] VerifyPaymentRequestDto requestDto);
    }
}
