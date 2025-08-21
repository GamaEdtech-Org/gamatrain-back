namespace GamaEdtech.Application.Service
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Data.Dto.Game;

    public class GameSerivce : IGameService
    {
        public Task<ResultData<int>> TakePointsAsync([NotNull] TakePointsDto requestDto) => throw new NotImplementedException();
    }
}
