namespace GamaEdtech.Backend.Shared.Service
{
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Specification;
    using System.Diagnostics.CodeAnalysis;

    using Farsica.Framework.DataAnnotation;

    using GamaEdtech.Backend.Data.Dto.School;
    using GamaEdtech.Backend.Data.Entity;

    [Injectable]
    public interface ISchoolService
    {
        Task<ResultData<ListDataSource<SchoolsDto>>> GetSchoolsAsync(ListRequestDto<School>? requestDto = null);
        Task<ResultData<SchoolDto>> GetSchoolAsync([NotNull] ISpecification<School> specification);
        Task<ResultData<int>> ManageSchoolAsync([NotNull] ManageSchoolRequestDto requestDto);
        Task<ResultData<bool>> RemoveSchoolAsync([NotNull] ISpecification<School> specification);
    }
}
