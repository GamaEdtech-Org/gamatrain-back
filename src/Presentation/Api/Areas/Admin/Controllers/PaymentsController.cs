namespace GamaEdtech.Presentation.Api.Areas.Admin.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.DataAccess.Specification.Impl;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Domain.Specification;
    using GamaEdtech.Presentation.ViewModel.Payment;

    using Microsoft.AspNetCore.Mvc;

    [Common.DataAnnotation.Area(nameof(Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    [Permission(Roles = [nameof(Role.Admin)])]
    public class PaymentsController(Lazy<ILogger<PaymentsController>> logger, Lazy<IPaymentService> transactionService)
        : ApiControllerBase<PaymentsController>(logger)
    {
        [HttpGet, Produces<ApiResponse<ListDataSource<PaymentsListResponseViewModel>>>()]
        public async Task<IActionResult<ListDataSource<PaymentsListResponseViewModel>>> GetPayments([NotNull, FromQuery] PaymentsListRequestViewModel request)
        {
            try
            {
                ISpecification<Payment>? specification = null;

                if (request.StartDate.HasValue || request.EndDate.HasValue)
                {
                    specification = new CreationDateBetweenSpecification<Payment>(request.StartDate, request.EndDate);
                }

                if (request.UserId.HasValue)
                {
                    var spec = new UserIdEqualsSpecification<Payment, int>(request.UserId.Value);
                    specification = specification is null ? spec : specification.And(spec);
                }

                var result = await transactionService.Value.GetPaymentsAsync(new ListRequestDto<Payment>
                {
                    PagingDto = request.PagingDto,
                    Specification = specification,
                });
                return Ok<ListDataSource<PaymentsListResponseViewModel>>(new()
                {
                    Errors = result.Errors,
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new PaymentsListResponseViewModel
                        {
                            Id = t.Id,
                            CreationDate = t.CreationDate,
                            UserId = t.UserId,
                            Amount = t.Amount,
                            Comment = t.Comment,
                            Currency = t.Currency,
                            FirstName = t.FirstName,
                            LastName = t.LastName,
                            SourceWallet = t.SourceWallet,
                            Status = t.Status,
                            TransactionId = t.TransactionId,
                            VerifyDate = t.VerifyDate,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<ListDataSource<PaymentsListResponseViewModel>>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }
    }
}
