namespace GamaEdtech.Backend.Shared.Service
{
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Specification;
    using System.Diagnostics.CodeAnalysis;

    using Farsica.Framework.DataAnnotation;

    using GamaEdtech.Backend.Data.Dto.Board;
    using GamaEdtech.Backend.Data.Entity;

    [Injectable]
    public interface IBoardService
    {
        Task<ResultData<ListDataSource<BoardsDto>>> GetBoardsAsync(ListRequestDto<Board>? requestDto = null);
        Task<ResultData<BoardDto>> GetBoardAsync([NotNull] ISpecification<Board> specification);
        Task<ResultData<int>> ManageBoardAsync([NotNull] ManageBoardRequestDto requestDto);
        Task<ResultData<bool>> RemoveBoardAsync([NotNull] ISpecification<Board> specification);
    }
}
