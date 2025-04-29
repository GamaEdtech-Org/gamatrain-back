namespace GamaEdtech.Application.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Data.Dto.Reaction;
    using GamaEdtech.Domain.Entity;

    [Injectable]
    public interface IReactionService
    {
        Task<ResultData<ReactionDto>> GetReactionsCountAsync([NotNull] ISpecification<Reaction> specification);
        Task<ResultData<bool>> ManageReactionAsync([NotNull] ManageReactionRequestDto requestDto);
        Task<ResultData<bool>> ExistReactionAsync([NotNull] ISpecification<Reaction> specification);
        Task<ResultData<bool>> RemoveReactionAsync([NotNull] ISpecification<Reaction> specification);
    }
}
