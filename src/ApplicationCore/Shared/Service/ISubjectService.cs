namespace GamaEdtech.Backend.Shared.Service
{
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Specification;
    using System.Diagnostics.CodeAnalysis;

    using Farsica.Framework.DataAnnotation;

    using GamaEdtech.Backend.Data.Dto.Subject;
    using GamaEdtech.Backend.Data.Entity;

    [Injectable]
    public interface ISubjectService
    {
        Task<ResultData<ListDataSource<SubjectsDto>>> GetSubjectsAsync(ListRequestDto<Subject>? requestDto = null);
        Task<ResultData<SubjectDto>> GetSubjectAsync([NotNull] ISpecification<Subject> specification);
        Task<ResultData<int>> ManageSubjectAsync([NotNull] ManageSubjectRequestDto requestDto);
        Task<ResultData<bool>> RemoveSubjectAsync([NotNull] ISpecification<Subject> specification);
    }
}
