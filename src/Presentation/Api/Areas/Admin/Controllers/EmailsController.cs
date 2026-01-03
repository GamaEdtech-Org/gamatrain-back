namespace GamaEdtech.Presentation.Api.Areas.Admin.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Presentation.ViewModel.Email;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    [Common.DataAnnotation.Area(nameof(Role.Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    [Permission(Roles = [nameof(Role.Admin)])]
    [Display(Name = "Emails")]
    public class EmailsController(Lazy<ILogger<EmailsController>> logger, Lazy<IEmailService> emailService, Lazy<IStringLocalizer<EmailsController>> localizer)
        : LocalizableApiControllerBase<EmailsController>(logger, localizer)
    {
        [HttpPost, Produces(typeof(ApiResponse<Void>))]
        [Display(Name = "Send Email")]
        public async Task<IActionResult> SendEmail([NotNull] SendEmailRequestViewModel request)
        {
            try
            {
                var result = await emailService.Value.SendEmailAsync(new()
                {
                    Sender = request.Sender!,
                    Subject = request.Subject!,
                    Body = request.Body!,
                    Users = request.Users!,
                    EmailAddresses = request.EmailAddresses,
                    CreationUserId = User.UserId(),
                    CreationDate = DateTimeOffset.UtcNow,
                });
                return Ok<Void>(new(result.Errors));
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok<Void>(new() { Errors = new[] { new Error { Message = exc.Message } } });
            }
        }
    }
}
