namespace GamaEdtech.Application.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using EntityFramework.Exceptions.Common;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;

    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Entity.Identity;

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
        public async Task<ResultData<string>> CreateRefrralUserAsync()
        {
            try
            {
                var userId = HttpContextAccessor.Value.HttpContext?.User.UserId();

                if (!userId.HasValue)
                {
                    return new(OperationResult.Failed)
                    {
                        Errors = new[] { new Error { Message = Localizer.Value["AuthenticationError"].Value } },
                    };
                }

                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var userRepo = uow.GetRepository<ApplicationUser, int>();

                var userInfo = await userRepo
                    .GetManyQueryable(u => u.Id == userId.Value)
                    .Select(u => new { u.FirstName, u.LastName })
                    .FirstOrDefaultAsync();

                if (userInfo == null)
                {
                    return new(OperationResult.NotFound)
                    {
                        Errors = [new() { Message = $"User not found. {userId}" }]
                    };
                }

                if (string.IsNullOrWhiteSpace(userInfo.FirstName) || string.IsNullOrWhiteSpace(userInfo.LastName))
                {
                    return new(OperationResult.NotValid)
                    {
                        Errors = [new() { Message = "User first name or last name is missing." }]
                    };
                }

                var uniqueSourceString = $"{userId}-{Guid.NewGuid()}";
                var bytes = System.Text.Encoding.UTF8.GetBytes(uniqueSourceString);
                var base64String = Convert.ToBase64String(bytes);
                var cleanString = base64String
                    .Replace("+", string.Empty, StringComparison.Ordinal)
                    .Replace("/", string.Empty, StringComparison.Ordinal)
                    .TrimEnd('=');
                var referralCode = cleanString[..10];

                var repository = uow.GetRepository<ReferralUser, int>();
                var exists = await repository.GetManyQueryable(t => t.ReferralId == referralCode).AnyAsync();
                if (exists)
                {
                    return new(OperationResult.Duplicate)
                    {
                        Errors = [new() { Message = Localizer.Value["ReferralIdAlreadyExists"] }]
                    };
                }

                var referralUser = new ReferralUser
                {
                    Name = userInfo.FirstName,
                    Family = userInfo.LastName,
                    ReferralId = referralCode,
                    CreationUserId = userId.Value,
                    CreationDate = DateTimeOffset.UtcNow
                };

                repository.Add(referralUser);
                _ = await uow.SaveChangesAsync();

                return new(OperationResult.Succeeded) { Data = referralCode };
            }
            catch (ReferenceConstraintException)
            {
                return new(OperationResult.NotValid)
                {
                    Errors = [new() { Message = Localizer.Value["ReferralUserConstraintError"] }]
                };
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
