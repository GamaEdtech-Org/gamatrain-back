namespace GamaEdtech.Infrastructure.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Data.Dto.Game;

    [Injectable]
    public interface ICoreProvider
    {
        Task<ResultData<bool>> ValidateTestAsync([NotNull] TestTimeRequestDto requestDto);
    }
}
