namespace GamaEdtech.Infrastructure.Provider.Email
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using System.Threading.Tasks;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.HttpProvider;
    using GamaEdtech.Data.Dto.Email;
    using GamaEdtech.Data.Dto.Provider.Email;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Infrastructure.Interface;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Resend;
    using Resend.Webhooks;

    using static GamaEdtech.Common.Core.Constants;
    using static GamaEdtech.Data.Dto.Email.EmailDto;

    public sealed class ResendEmailProvider(Lazy<ILogger<ResendEmailProvider>> logger, Lazy<IConfiguration> configuration, Lazy<IHttpProvider> httpProvider) : IEmailProvider
    {
        public EmailProviderType ProviderType => EmailProviderType.Resend;

        public async Task<ResultData<Common.Data.Void>> SendEmailAsync([NotNull] EmailRequestDto requestDto)
        {
            try
            {
                var client = CreateClient();
                var chunks = requestDto.Receivers.Chunk(100);
                List<string?> errors = [];
                foreach (var item in chunks)
                {
                    var response = await client.EmailBatchAsync(item.Select(t => new EmailMessage
                    {
                        From = $"{requestDto.SenderName} <{Email}>",
                        HtmlBody = requestDto.Body,
                        Subject = requestDto.Subject,
                        To = EmailAddressList.From(t),
                        ReplyTo = Email,
                    }));
                    if (!response.Success)
                    {
                        errors.Add(response.Exception?.Message);
                    }
                }

                return new(errors.Count == 0 ? OperationResult.Succeeded : OperationResult.Failed)
                {
                    Errors = errors.Count == 0 ? null : errors.Select(t => new Error { Message = t, }),
                };
            }
            catch (Exception exc)
            {
                logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<EmailDto>> ProccessInboundEmailAsync([NotNull] HttpRequest request)
        {
            try
            {
                var options = Options.Create<WebhookValidatorOptions>(new()
                {
                    Secret = configuration.Value.GetValue<string>("EmailProvider:Resend:Secret")!
                });

                var payload = (await request.ReadFromJsonAsync<object>())?.ToString();
                var validationResult = new WebhookValidator(options).Validate(request, payload!);
                logger.Value.LogException(new Exception("Resend 3"));
                if (!validationResult.IsValid)
                {
                    logger.Value.LogException(new Exception($"Resend 4: {JsonSerializer.Serialize(validationResult)}"));
                    return new(OperationResult.Failed) { Errors = [new() { Message = validationResult.Exception?.Message, }] };
                }

                logger.Value.LogException(new Exception("Resend 5"));
                var data = await request.ReadFromJsonAsync<ResendEmailResponse>();
                logger.Value.LogException(new Exception($"Resend 6: {JsonSerializer.Serialize(data)}"));
                var client = CreateClient();
                logger.Value.LogException(new Exception($"Resend 7"));
                var content = await client.EmailRetrieveAsync(data!.Data.EmailId);
                logger.Value.LogException(new Exception($"Resend 8: {JsonSerializer.Serialize(content)}"));
                List<AttachmentDto>? lst = [];
                if (data.Data.Attachments?.Any() == true)
                {
                    logger.Value.LogException(new Exception($"Resend 9"));
                    foreach (var item in data.Data.Attachments)
                    {
                        logger.Value.LogException(new Exception($"Resend 10"));
                        var attachment = await client.EmailAttachmentRetrieveAsync(data!.Data.EmailId, item.Id);
                        logger.Value.LogException(new Exception($"Resend 11: {JsonSerializer.Serialize(attachment)}"));
                        if (string.IsNullOrEmpty(attachment.Content?.DownloadUrl))
                        {
                            continue;
                        }

                        logger.Value.LogException(new Exception($"Resend 12"));
                        var response = await httpProvider.Value.GetByteArrayAsync<IHttpRequest, IHttpRequest>(new()
                        {
                            Uri = attachment.Content.DownloadUrl,
                            Request = null,
                        });
                        logger.Value.LogException(new Exception($"Resend 13"));
                        if (response is not null)
                        {
                            logger.Value.LogException(new Exception($"Resend 14: {JsonSerializer.Serialize(response)}"));
                            lst.Add(new()
                            {
                                File = response,
                                Filename = item.Filename,
                                ContentType = item.ContentType,
                            });
                        }
                    }
                }
                logger.Value.LogException(new Exception($"Resend 15"));
                return new(OperationResult.Succeeded)
                {
                    Data = new()
                    {
                        Body = content.Content.TextBody,
                        From = data.Data.From,
                        Subject = data.Data.Subject,
                        Attachments = lst,
                    }
                };
            }
            catch (Exception exc)
            {
                logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        private IResend CreateClient() => ResendClient.Create(configuration.Value.GetValue<string>("EmailProvider:Resend:ApiToken")!);

        private string Email => configuration.Value.GetValue<string>("EmailProvider:Resend:Email")!;
    }
}
