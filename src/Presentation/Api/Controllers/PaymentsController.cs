namespace GamaEdtech.Presentation.Api.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Presentation.ViewModel.Payment;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Permission(policy: null)]
    public class PaymentsController(Lazy<ILogger<PaymentsController>> logger, Lazy<IPaymentService> paymentService)
        : ApiControllerBase<PaymentsController>(logger)
    {
        [HttpPost, Produces(typeof(ApiResponse<long>))]
        [Permission(policy: null)]
        public async Task<IActionResult<long>> CreatePayment([NotNull] CreatePaymentRequestViewModel request)
        {
            try
            {
                var result = await paymentService.Value.CreatePaymentAsync(new()
                {
                    UserId = User.UserId(),
                    Amount = request.Amount.GetValueOrDefault(),
                    Currency = request.Currency!,
                });

                return Ok<long>(new(result.Errors)
                {
                    Data = result.Data,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return Ok<long>(new(new Error { Message = exc.Message }));
            }
        }

        [HttpPost("verify"), Produces(typeof(ApiResponse<bool>))]
        [Permission(policy: null)]
        public async Task<IActionResult<bool>> VerifyPayment([NotNull] VerifyPaymentRequestViewModel request)
        {
            try
            {
                var result = await paymentService.Value.VerifyPaymentAsync(new()
                {
                    Currency = request.Currency!,
                    Id = request.Id.GetValueOrDefault(),
                    TransactionId = request.TransactionId!,
                    UserId = User.UserId(),
                });

                return Ok<bool>(new(result.Errors)
                {
                    Data = result.OperationResult is Constants.OperationResult.Succeeded,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return Ok<bool>(new(new Error { Message = exc.Message }));
            }
        }
    }
}
