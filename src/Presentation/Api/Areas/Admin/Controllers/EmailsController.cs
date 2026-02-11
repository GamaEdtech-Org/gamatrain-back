namespace GamaEdtech.Presentation.Api.Areas.Admin.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification.Impl;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Common.Resources;
    using GamaEdtech.Domain.Entity.Identity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Presentation.ViewModel.Email;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    [Common.DataAnnotation.Area(nameof(Role.Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    [Permission(Roles = [nameof(Role.Admin)])]
    [Display(Name = "Emails")]
    public class EmailsController(Lazy<ILogger<EmailsController>> logger, Lazy<IEmailService> emailService, Lazy<IStringLocalizer<EmailsController>> localizer, Lazy<IIdentityService> identityService)
        : LocalizableApiControllerBase<EmailsController>(logger, localizer)
    {
        [HttpPost, Produces(typeof(ApiResponse<Void>))]
        [Display(Name = "Send Email")]
        public async Task<IActionResult> SendEmail([NotNull] SendEmailRequestViewModel request)
        {
            try
            {
                List<string> emails = [];
                if (request.EmailAddresses is not null)
                {
                    emails.AddRange(request.EmailAddresses);
                }

                if (request.Users?.Any() == true)
                {
                    var data = await identityService.Value.GetUsersEmailAsync(new IdContainsSpecification<ApplicationUser, int>(request.Users));
                    if (data.OperationResult is not Constants.OperationResult.Succeeded)
                    {
                        return Ok<Void>(new() { Errors = data.Errors });
                    }

                    emails.AddRange(data.Data!);
                }

                if (emails.Count == 0)
                {
                    var msg = GlobalResource.Validation_Required;
                    return Ok<Void>(new() { Errors = new[] { new Error { Message = string.Format(msg, Globals.DisplayNameFor<SendEmailRequestViewModel>(t => t.EmailAddresses!)) } } });
                }

                var result = await emailService.Value.SendEmailAsync(new()
                {
                    SenderName = request.SenderName!,
                    Subject = request.Subject!,
                    Body = request.Body!,
                    EmailAddresses = emails,
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
