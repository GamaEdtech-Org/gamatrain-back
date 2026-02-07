namespace GamaEdtech.Application.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Common.Service.Factory;
    using GamaEdtech.Data.Dto.Email;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Infrastructure.Interface;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    public class EmailService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<EmailService>> localizer
        , Lazy<ILogger<EmailService>> logger, Lazy<IGenericFactory<IEmailProvider, EmailProviderType>> genericFactory, Lazy<IConfiguration> configuration)
        : LocalizableServiceBase<EmailService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IEmailService
    {
        private IEmailProvider EmailProvider
        {
            get
            {
                _ = configuration.Value.GetValue<string?>("EmailProvider:Type").TryGetFromNameOrValue<EmailProviderType, byte>(out var emailProviderType);
                return genericFactory.Value.GetProvider(emailProviderType!)!;
            }
        }

        public async Task<ResultData<Common.Data.Void>> SendEmailAsync([NotNull] SendEmailRequestDto requestDto)
        {
            try
            {
                return await EmailProvider.SendEmailAsync(new()
                {
                    SenderName = requestDto.SenderName,
                    Body = requestDto.Body,
                    Subject = requestDto.Subject,
                    Receivers = requestDto.EmailAddresses,
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<EmailDto>> ProccessInboundEmailAsync([NotNull] HttpRequest request)
        {
            try
            {
                return await EmailProvider.ProccessInboundEmailAsync(request);
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }
    }
}
