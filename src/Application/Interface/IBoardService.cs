namespace GamaEdtech.Application.Interface
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.DataAnnotation;

    using GamaEdtech.Data.Dto.Board;
    using GamaEdtech.Domain.Entity;

    [Injectable]
    public interface IBoardService
    {
        Task<ResultData<ListDataSource<BoardsDto>>> GetBoardsAsync(ListRequestDto<Board>? requestDto = null);
        Task<ResultData<BoardDto>> GetBoardAsync([NotNull] ISpecification<Board> specification);
        Task<ResultData<int>> ManageBoardAsync([NotNull] ManageBoardRequestDto requestDto);
        Task<ResultData<bool>> RemoveBoardAsync([NotNull] ISpecification<Board> specification);
    }
}
