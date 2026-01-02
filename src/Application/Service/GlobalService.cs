namespace GamaEdtech.Application.Service
{
    using System;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Data.Enumeration;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Common.Service.Factory;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Infrastructure.Interface;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    using Error = Common.Data.Error;

    public class GlobalService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<GlobalService>> localizer, Lazy<ILogger<GlobalService>> logger
            , Lazy<IGenericFactory<ICaptchaProvider, CaptchaProviderType>> genericFactory, Lazy<IConfiguration> configuration)
        : LocalizableServiceBase<GlobalService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IGlobalService
    {
        public async Task<ResultData<bool>> VerifyCaptchaAsync(string? captcha)
        {
            try
            {
                _ = configuration.Value.GetValue<string?>("Captcha:Type").TryGetFromNameOrValue<CaptchaProviderType, byte>(out var captchaProviderType);

                return await genericFactory.Value.GetProvider(captchaProviderType!)!.VerifyCaptchaAsync(captcha);
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = new[] { new Error { Message = exc.Message }, } };
            }
        }
    }
}
