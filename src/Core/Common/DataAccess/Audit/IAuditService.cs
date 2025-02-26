namespace GamaEdtech.Common.DataAccess.Audit
{
    using System.Threading.Tasks;

    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.Data;

    [Injectable]
    public interface IAuditService
    {
        Task<ResultData<ListDataSource<AuditListDto>>> GetAuditsAsync(ListRequestDto<Audit>? requestDto = null);

        Task<ResultData<AuditDto>> GetAsync(ISpecification<Audit> specification);
    }
}
