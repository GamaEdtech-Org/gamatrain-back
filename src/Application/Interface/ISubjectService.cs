namespace GamaEdtech.Application.Interface
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.DataAnnotation;

    using GamaEdtech.Data.Dto.Subject;
    using GamaEdtech.Domain.Entity;

    [Injectable]
    public interface ISubjectService
    {
        Task<ResultData<ListDataSource<SubjectsDto>>> GetSubjectsAsync(ListRequestDto<Subject>? requestDto = null);
        Task<ResultData<SubjectDto>> GetSubjectAsync([NotNull] ISpecification<Subject> specification);
        Task<ResultData<int>> ManageSubjectAsync([NotNull] ManageSubjectRequestDto requestDto);
        Task<ResultData<bool>> RemoveSubjectAsync([NotNull] ISpecification<Subject> specification);
    }
}
