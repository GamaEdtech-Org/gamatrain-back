namespace GamaEdtech.Application.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using EntityFramework.Exceptions.Common;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Core.Extensions.Linq;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;

    using GamaEdtech.Domain.Entity;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    public class ReferralService(
        Lazy<IUnitOfWorkProvider> unitOfWorkProvider,
        Lazy<IHttpContextAccessor> httpContextAccessor,
        Lazy<IStringLocalizer<ReferralService>> localizer,
        Lazy<ILogger<ReferralService>> logger)
        : LocalizableServiceBase<ReferralService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IReferralService
    {
        public async Task<ResultData<bool>> CreateRefrralUserAsync(ReferralUser referralUser)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<ReferralUser, int>();

                // Check for duplicate ReferralId
                var exists = await repository.GetManyQueryable(t => t.ReferralId == referralUser.ReferralId).AnyAsync();
                if (exists)
                {
                    return new(OperationResult.Duplicate)
                    {
                        Data = false,
                        Errors = [new() { Message = Localizer.Value["ReferralIdAlreadyExists"] }]
                    };
                }

                repository.Add(referralUser);
                _ = await uow.SaveChangesAsync();

                return new(OperationResult.Succeeded) { Data = true };
            }
            catch (ReferenceConstraintException)
            {
                return new(OperationResult.NotValid)
                {
                    Data = false,
                    Errors = [new() { Message = Localizer.Value["ReferralUserConstraintError"] }]
                };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed)
                {
                    Data = false,
                    Errors = [new() { Message = exc.Message }]
                };
            }
        }

        public async Task<ResultData<ReferralUser>> GetReferralUserAsync([NotNull] ISpecification<ReferralUser> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var referralUser = await uow.GetRepository<ReferralUser, int>().GetAsync(specification);

                return referralUser is null
                    ? new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = Localizer.Value["ReferralUserNotFound"] }]
                    }
                    : new(OperationResult.Succeeded) { Data = referralUser };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed)
                {
                    Errors = [new() { Message = exc.Message }]
                };
            }
        }

        public async Task<ResultData<bool>> RemoveReferralUserAsync([NotNull] ISpecification<ReferralUser> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var referralUser = await uow.GetRepository<ReferralUser, int>().GetAsync(specification);
                if (referralUser is null)
                {
                    return new(OperationResult.NotFound)
                    {
                        Data = false,
                        Errors = [new() { Message = Localizer.Value["ReferralUserNotFound"] }]
                    };
                }

                uow.GetRepository<ReferralUser, int>().Remove(referralUser);
                _ = await uow.SaveChangesAsync();
                return new(OperationResult.Succeeded) { Data = true };
            }
            catch (ReferenceConstraintException)
            {
                return new(OperationResult.NotValid)
                {
                    Data = false,
                    Errors = [new() { Message = Localizer.Value["ReferralUserCantBeRemoved"] }]
                };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed)
                {
                    Data = false,
                    Errors = [new() { Message = exc.Message }]
                };
            }
        }
    }
}
