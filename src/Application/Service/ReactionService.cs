namespace GamaEdtech.Application.Service
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.DataAccess.Specification.Impl;
    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.Service;
    using GamaEdtech.Data.Dto.Reaction;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Entity.Identity;
    using GamaEdtech.Domain.Specification;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;

    using static GamaEdtech.Common.Core.Constants;

    public class ReactionService(Lazy<IUnitOfWorkProvider> unitOfWorkProvider, Lazy<IHttpContextAccessor> httpContextAccessor, Lazy<IStringLocalizer<ReactionService>> localizer
        , Lazy<ILogger<ReactionService>> logger)
        : LocalizableServiceBase<ReactionService>(unitOfWorkProvider, httpContextAccessor, localizer, logger), IReactionService
    {
        public async Task<ResultData<ReactionDto>> GetReactionsCountAsync([NotNull] ISpecification<Reaction> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var reaction = await uow.GetRepository<Reaction>().GetManyQueryable(specification)
                    .GroupBy(t => t.IsLike).Select(t => new
                    {
                        t.Key,
                        Count = t.Count(),
                    }).ToListAsync();

                return new(OperationResult.Succeeded)
                {
                    Data = new ReactionDto
                    {
                        Like = reaction.Find(t => t.Key)?.Count ?? 0,
                        Dislike = reaction.Find(t => !t.Key)?.Count ?? 0,
                    }
                };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message },] };
            }
        }

        public async Task<ResultData<bool>> ManageReactionAsync([NotNull] ManageReactionRequestDto requestDto)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<Reaction>();

                var specification = new CategoryTypeEqualsSpecification<Reaction>(requestDto.CategoryType)
                    .And(new IdentifierIdEqualsSpecification<Reaction>(requestDto.IdentifierId))
                    .And(new CreationUserIdEqualsSpecification<Reaction, ApplicationUser, int>(requestDto.CreationUserId));

                var reaction = await repository.GetAsync(specification);
                if (reaction is not null)
                {
                    if (reaction.IsLike != requestDto.IsLike)
                    {
                        reaction.IsLike = requestDto.IsLike;
                        reaction.CreationDate = requestDto.CreationDate;

                        _ = repository.Update(reaction);
                    }
                }
                else
                {
                    reaction = new Reaction
                    {
                        CategoryType = requestDto.CategoryType,
                        IdentifierId = requestDto.IdentifierId,
                        CreationUserId = requestDto.CreationUserId,
                        CreationDate = requestDto.CreationDate,
                        IsLike = requestDto.IsLike,
                    };
                    repository.Add(reaction);
                }

                _ = await uow.SaveChangesAsync();

                return new(OperationResult.Succeeded) { Data = true };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<bool>> ExistReactionAsync([NotNull] ISpecification<Reaction> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<Reaction>();

                return new(OperationResult.Succeeded) { Data = await repository.AnyAsync(specification) };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }

        public async Task<ResultData<bool>> RemoveReactionAsync([NotNull] ISpecification<Reaction> specification)
        {
            try
            {
                var uow = UnitOfWorkProvider.Value.CreateUnitOfWork();
                var repository = uow.GetRepository<Reaction>();

                _ = await repository.GetManyQueryable(specification).ExecuteDeleteAsync();

                return new(OperationResult.Succeeded) { Data = true };
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);
                return new(OperationResult.Failed) { Errors = [new() { Message = exc.Message, }] };
            }
        }
    }
}
