namespace GamaEdtech.Application.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Data.Dto.Game;

    [Injectable]
    public interface IGameService
    {
        Task<ResultData<int>> TakePointsAsync([NotNull] TakePointsDto requestDto);
        Task<IReadOnlyList<string>> GenerateCoinsAsync();
    }
}
