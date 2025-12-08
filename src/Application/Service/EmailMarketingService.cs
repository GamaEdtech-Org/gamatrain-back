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
    using GamaEdtech.Data.Dto.EmailMarketing;
    using GamaEdtech.Domain.Entity.Identity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Infrastructure.Interface;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    public class EmailMarketingService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<EmailMarketingService>> localizer
        , Lazy<ILogger<EmailMarketingService>> logger, Lazy<IGenericFactory<IEmailProvider, EmailProviderType>> genericFactory, Lazy<IConfiguration> configuration)
        : LocalizableServiceBase<EmailMarketingService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IEmailMarketingService
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
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var emails = await uow.GetRepository<ApplicationUser, int>().GetManyQueryable(t => requestDto.Users.Contains(t.Id)).Select(t => t.Email!).ToListAsync();

                return await EmailProvider.SendEmailAsync(new()
                {
                    Body = requestDto.Body,
                    Subject = requestDto.Subject,
                    Receivers = emails.Concat(requestDto.EmailAddresses ?? []),
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }
    }
}
