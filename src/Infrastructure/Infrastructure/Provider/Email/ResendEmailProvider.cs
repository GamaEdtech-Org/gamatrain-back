namespace GamaEdtech.Infrastructure.Provider.Email
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Data.Dto.Email;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Infrastructure.Interface;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Resend;

    using static GamaEdtech.Common.Core.Constants;

    public sealed class ResendEmailProvider(Lazy<ILogger<ResendEmailProvider>> logger, Lazy<IConfiguration> configuration) : IEmailProvider
    {
        public EmailProviderType ProviderType => EmailProviderType.Resend;

        public async Task<ResultData<Common.Data.Void>> SendEmailAsync([NotNull] EmailRequestDto requestDto)
        {
            try
            {
                var resend = ResendClient.Create(configuration.Value.GetValue<string>("EmailProvider:Resend:ApiToken")!);
                var chunks = requestDto.Receivers.Chunk(100);
                List<string?> errors = [];
                foreach (var item in chunks)
                {
                    var response = await resend.EmailBatchAsync(item.Select(t => new EmailMessage
                    {
                        From = requestDto.Sender,
                        HtmlBody = requestDto.Body,
                        Subject = requestDto.Subject,
                        To = EmailAddressList.From(t),
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
    }
}
