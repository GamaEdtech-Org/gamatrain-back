namespace GamaEdtech.Application.Service
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using EntityFramework.Exceptions.Common;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Data.Dto.UserReferral;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Entity.Identity;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    using System.Security.Cryptography;

    public class ReferralService(
        Lazy<IUnitOfWorkProvider> unitOfWorkProvider,
        Lazy<IHttpContextAccessor> httpContextAccessor,
        Lazy<IStringLocalizer<ReferralService>> localizer,
        Lazy<ILogger<ReferralService>> logger)
        : LocalizableServiceBase<ReferralService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IReferralService
    {
        public async Task<ResultData<string>> CreateReferralUserAsync()
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
                        Errors = [new() { Message = "User not found" }]
                    };
                }

                if (string.IsNullOrWhiteSpace(userInfo.FirstName) || string.IsNullOrWhiteSpace(userInfo.LastName))
                {
                    return new(OperationResult.NotValid)
                    {
                        Errors = [new() { Message = "User first name or last name is missing." }]
                    };
                }

                var uniqueSource = $"{userId}-{DateTimeOffset.UtcNow.Ticks}-{Guid.NewGuid()}";
                var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(uniqueSource));
                var referralCode = ToBase32(hashBytes)[..10];


                var repository = uow.GetRepository<ReferralUser, int>();


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
        public async Task<ResultData<ListDataSource<UserReferralDto>>> GetAllUsersReferralAsync()
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var referralRepo = uow.GetRepository<ReferralUser, int>();

                var allReferralUsers = await referralRepo.GetAllAsync();

                if (allReferralUsers == null)
                {
                    return new(OperationResult.Succeeded)
                    {
                        Data = new ListDataSource<UserReferralDto>()
                    };
                }

                var data = allReferralUsers
                    .Select(r => new UserReferralDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Family = r.Family,
                        ReferralId = r.ReferralId,
                        CreationDate = r.CreationDate
                    })
                    .ToList();
                return new(OperationResult.Succeeded) { Data = new() { List = data, TotalRecordsCount = data.Count } };
            }
            catch (Exception ex)
            {
                Logger.Value.LogException(ex);
                return new(OperationResult.Failed)
                {
                    Errors = [new() { Message = ex.Message }]
                };
            }
        }

        private static string ToBase32(byte[] data)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            StringBuilder result = new();

            int buffer = data[0];
            var next = 1;
            var bitsLeft = 8;
            while (bitsLeft > 0 || next < data.Length)
            {
                if (bitsLeft < 5)
                {
                    if (next < data.Length)
                    {
                        buffer <<= 8;
                        buffer |= data[next++] & 0xFF;
                        bitsLeft += 8;
                    }
                    else
                    {
                        var pad = 5 - bitsLeft;
                        buffer <<= pad;
                        bitsLeft += pad;
                    }
                }

                var index = (buffer >> (bitsLeft - 5)) & 0x1F;
                bitsLeft -= 5;
                _ = result.Append(alphabet[index]);
            }

            return result.ToString();
        }

    }
}
