namespace GamaEdtech.Presentation.Api.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;

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
        public async Task<IActionResult<ManageTicketResponseViewModel>> CreateTicket([NotNull, FromForm] CreateTicketRequestViewModel request)
        {
            try
            {
                var validateCaptcha = await globalService.Value.VerifyCaptchaAsync(request.Captcha);
                if (!validateCaptcha.Data)
                {
                    return Ok<ManageTicketResponseViewModel>(new(new Error { Message = "Invalid Captcha" }));
                }

                int? userId = User.Identity?.IsAuthenticated == true ? User.UserId() : null;
                var result = await ticketService.Value.CreateTicketAsync(new()
                {
                    Body = request.Body,
                    Email = request.Email,
                    FullName = request.FullName,
                    Subject = request.Subject,
                    CreationUserId = userId,
                    File = request.File,
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

        [HttpPost("inbound-webhook"), Produces<ApiResponse<Void>>()]
        public async Task<IActionResult<Void>> InboundWebHook()
        {
            try
            {
                Logger.Value.LogException(new Exception("InboundWebHook 1"));

                Logger.Value.LogException(new Exception($"InboundWebHook 3: {JsonSerializer.Serialize(Request.Headers)}"));
                var from = await Request.ReadFormAsync();
                Logger.Value.LogException(new Exception($"InboundWebHook 4: {JsonSerializer.Serialize(from)}"));

                await ticketService.Value.ProccessInboundEmailAsync(Request);
                Logger.Value.LogException(new Exception("InboundWebHook 2"));
                return Ok<Void>(new());
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return Ok<Void>(new(new Error { Message = exc.Message }));
            }
        }
    }
}
