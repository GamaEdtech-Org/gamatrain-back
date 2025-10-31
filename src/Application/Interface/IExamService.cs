namespace GamaEdtech.Application.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Data.Dto.Game;

    [Injectable]
    public interface IExamService
    {
        Task<ResultData<ExportExamResponseDto>> ExportExamAsync([NotNull] ExportExamRequestDto requestDto);
    }
}
