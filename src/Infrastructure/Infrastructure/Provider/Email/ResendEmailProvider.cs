namespace GamaEdtech.Infrastructure.Provider.Email
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
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
                request.EnableBuffering();

                // Read raw payload
                using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                var payload = await reader.ReadToEndAsync();
                request.Body.Position = 0;

                logger.Value.LogException(new Exception($"Resend Payload RAW => {payload}"));

                var options = Options.Create(new WebhookValidatorOptions
                {
                    Secret = configuration.Value.GetValue<string>("EmailProvider:Resend:Secret")!
                });

                var validationResult = new WebhookValidator(options).Validate(request, payload);

                if (!validationResult.IsValid)
                {
                    logger.Value.LogException(new Exception($"Resend Validation Failed => {JsonSerializer.Serialize(validationResult)}"));

                    return new(OperationResult.Failed)
                    {
                        Errors =
                        [
                            new() { Message = validationResult.Exception?.Message }
                        ]
                    };
                }

                // Deserialize from same payload
                var data = JsonSerializer.Deserialize<ResendEmailResponse>(payload);

                logger.Value.LogException(new Exception($"Resend Webhook Data => {JsonSerializer.Serialize(data)}"));

                var client = CreateClient();
                var content = await client.EmailRetrieveAsync(data!.Data.EmailId);

                List<AttachmentDto> lst = [];

                if (data.Data.Attachments?.Any() == true)
                {
                    foreach (var item in data.Data.Attachments)
                    {
                        var attachment = await client.EmailAttachmentRetrieveAsync(data.Data.EmailId, item.Id);

                        if (string.IsNullOrEmpty(attachment.Content?.DownloadUrl))
                        {
                            continue;
                        }

                        var response = await httpProvider.Value.GetByteArrayAsync<IHttpRequest, IHttpRequest>(new()
                        {
                            Uri = attachment.Content.DownloadUrl,
                            Request = null,
                        });

                        if (response is not null)
                        {
                            lst.Add(new()
                            {
                                File = response,
                                Filename = item.Filename,
                                ContentType = item.ContentType,
                            });
                        }
                    }
                }

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

                return new(OperationResult.Failed)
                {
                    Errors =
                    [
                        new() { Message = exc.Message }
                    ]
                };
            }
        }

        private IResend CreateClient() => ResendClient.Create(configuration.Value.GetValue<string>("EmailProvider:Resend:ApiToken")!);

        private string Email => configuration.Value.GetValue<string>("EmailProvider:Resend:Email")!;
    }
}
