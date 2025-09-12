namespace GamaEdtech.Application.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Data.Dto.Payment;

    [Injectable]
    public interface IPaymentService
    {
        Task<ResultData<long>> CreatePaymentAsync([NotNull] CreatePaymentRequestDto requestDto);
        Task<ResultData<bool>> VerifyPaymentAsync([NotNull] VerifyPaymentRequestDto requestDto);
    }
}
