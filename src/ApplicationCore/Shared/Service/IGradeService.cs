namespace GamaEdtech.Backend.Shared.Service
{
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Specification;
    using System.Diagnostics.CodeAnalysis;

    using Farsica.Framework.DataAnnotation;

    using GamaEdtech.Backend.Data.Dto.Grade;
    using GamaEdtech.Backend.Data.Entity;

    [Injectable]
    public interface IGradeService
    {
        Task<ResultData<ListDataSource<GradesDto>>> GetGradesAsync(ListRequestDto<Grade>? requestDto = null);
        Task<ResultData<GradeDto>> GetGradeAsync([NotNull] ISpecification<Grade> specification);
        Task<ResultData<int>> ManageGradeAsync([NotNull] ManageGradeRequestDto requestDto);
        Task<ResultData<bool>> RemoveGradeAsync([NotNull] ISpecification<Grade> specification);
    }
}
