namespace GamaEdtech.Presentation.Api.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Presentation.ViewModel.Ticket;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TicketsController(Lazy<ILogger<TicketsController>> logger, Lazy<ITicketService> ticketService
        , Lazy<IGlobalService> globalService)
        : ApiControllerBase<TicketsController>(logger)
    {
        [HttpPost, Produces<ApiResponse<ManageTicketResponseViewModel>>()]
        public async Task<IActionResult<ManageTicketResponseViewModel>> CreateTicket([NotNull, FromBody] CreateTicketRequestViewModel request)
        {
            try
            {
                var validateCaptcha = await globalService.Value.VerifyCaptchaAsync(request.Captcha);
                if (!validateCaptcha.Data)
                {
                    return Ok<ManageTicketResponseViewModel>(new(new Error { Message = "Invalid Captcha" }));
                }

                var result = await ticketService.Value.CreateTicketAsync(new()
                {
                    Body = request.Body,
                    Email = request.Email,
                    Sender = request.FullName,
                    Subject = request.Subject,
                });
                return Ok<ManageTicketResponseViewModel>(new(result.Errors)
                {
                    Data = new() { Id = result.Data },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<ManageTicketResponseViewModel>(new(new Error { Message = exc.Message }));
            }
        }
    }
}
