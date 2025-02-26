namespace GamaEdtech.Application.Interface
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.DataAnnotation;

    using GamaEdtech.Data.Dto.Grade;
    using GamaEdtech.Domain.Entity;

    [Injectable]
    public interface IGradeService
    {
        Task<ResultData<ListDataSource<GradesDto>>> GetGradesAsync(ListRequestDto<Grade>? requestDto = null);
        Task<ResultData<GradeDto>> GetGradeAsync([NotNull] ISpecification<Grade> specification);
        Task<ResultData<int>> ManageGradeAsync([NotNull] ManageGradeRequestDto requestDto);
        Task<ResultData<bool>> RemoveGradeAsync([NotNull] ISpecification<Grade> specification);
    }
}
