namespace GamaEdtech.Infrastructure.Provider.Email
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
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

    using Resend;

    using Svix.Exceptions;

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
                logger.Value.LogException(new Exception("Resend 1"));
                if (!request.Body.CanSeek)
                {
                    logger.Value.LogException(new Exception("Resend 2"));
                    request.EnableBuffering();
                    logger.Value.LogException(new Exception("Resend 3"));
                }
                logger.Value.LogException(new Exception("Resend 4"));
                request.Body.Position = 0;
                logger.Value.LogException(new Exception("Resend 5"));
                using var reader = new StreamReader(request.Body, Encoding.UTF8);
                logger.Value.LogException(new Exception("Resend 6"));
                var payload = await reader.ReadToEndAsync();
                logger.Value.LogException(new Exception("Resend 7"));
                request.Body.Position = 0;
                logger.Value.LogException(new Exception("Resend 8"));

                if (!request.Headers.TryGetValue("svix-id", out var svixId))
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = "Missing required header 'svix-id'", }] };
                }

                if (!request.Headers.TryGetValue("svix-timestamp", out var svixTimestamp))
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = "Missing required header 'svix-timestamp'", }] };
                }

                if (!request.Headers.TryGetValue("svix-signature", out var svixSignature))
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = "Missing required header 'svix-signature'", }] };
                }

                if (!long.TryParse(svixTimestamp.ToString(), out _))
                {
                    return new(OperationResult.Failed) { Errors = [new() { Message = "Invalid value 'svix-timestamp', expected long", }] };
                }

                try
                {
                    logger.Value.LogException(new Exception("Resend 9"));
                    var webhook = new Svix.Webhook(configuration.Value.GetValue<string>("EmailProvider:Resend:Secret")!);
                    webhook.Verify(payload, new WebHeaderCollection
                    {
                        { "svix-id", svixId.ToString() },
                        { "svix-timestamp", svixTimestamp.ToString() },
                        { "svix-signature", svixSignature.ToString() }
                    });
                    logger.Value.LogException(new Exception("Resend 10"));
                }
                catch (WebhookVerificationException)
                {
                    logger.Value.LogException(new Exception("Resend 11"));
                    return new(OperationResult.Failed) { Errors = [new() { Message = "Invalid Signature", }] };
                }

                logger.Value.LogException(new Exception("Resend 12"));
                var data = await request.ReadFromJsonAsync<Data.Dto.Provider.Email.ResendResponse<ResendEmailReceivedWebhookDto>>();
                if (!"email.received".Equals(data?.Type, StringComparison.OrdinalIgnoreCase) || data?.Data is null)
                {
                    logger.Value.LogException(new Exception("Resend 13"));
                    return new(OperationResult.Succeeded);
                }

                logger.Value.LogException(new Exception($"Resend 14: {JsonSerializer.Serialize(data)}"));
                var client = CreateClient();
                logger.Value.LogException(new Exception($"Resend 15"));
                var content = await client.ReceivedEmailRetrieveAsync(data!.Data.EmailId);
                logger.Value.LogException(new Exception($"Resend 16: {JsonSerializer.Serialize(content)}"));
                List<AttachmentDto>? lst = [];
                if (data.Data.Attachments?.Any() == true)
                {
                    logger.Value.LogException(new Exception($"Resend 10"));
                    foreach (var item in data.Data.Attachments)
                    {
                        logger.Value.LogException(new Exception($"Resend 11"));
                        var attachment = await client.EmailAttachmentRetrieveAsync(data!.Data.EmailId, item.Id);
                        logger.Value.LogException(new Exception($"Resend 12: {JsonSerializer.Serialize(attachment)}"));
                        if (string.IsNullOrEmpty(attachment.Content?.DownloadUrl))
                        {
                            continue;
                        }

                        logger.Value.LogException(new Exception($"Resend 13"));
                        var response = await httpProvider.Value.GetByteArrayAsync<IHttpRequest, IHttpRequest>(new()
                        {
                            Uri = attachment.Content.DownloadUrl,
                            Request = null,
                        });
                        logger.Value.LogException(new Exception($"Resend 14"));
                        if (response is not null)
                        {
                            logger.Value.LogException(new Exception($"Resend 15: {JsonSerializer.Serialize(response)}"));
                            lst.Add(new()
                            {
                                File = response,
                                Filename = item.Filename,
                                ContentType = item.ContentType,
                            });
                        }
                    }
                }
                logger.Value.LogException(new Exception($"Resend 17"));
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
                logger.Value.LogException(new Exception($"Resend 18"));
                logger.Value.LogException(exc);
                logger.Value.LogException(new Exception($"Resend 19"));
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        private IResend CreateClient() => ResendClient.Create(configuration.Value.GetValue<string>("EmailProvider:Resend:ApiToken")!);

        private string Email => configuration.Value.GetValue<string>("EmailProvider:Resend:Email")!;
    }
}
