namespace GamaEdtech.Application.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Data.Dto.SiteMap;
    using GamaEdtech.Domain.Entity;

    [Injectable]
    public interface IGlobalService
    {
        Task<ResultData<bool>> VerifyCaptchaAsync(string? captcha);

        Task<ResultData<ListDataSource<SiteMapDto>>> GetSiteMapsAsync(ListRequestDto<SiteMap>? requestDto = null);
        Task<ResultData<long>> ManageSiteMapAsync([NotNull] ManageSiteMapRequestDto requestDto);
        Task<ResultData<bool>> RemoveSiteMapAsync([NotNull] ISpecification<SiteMap> specification);
        Task<ResultData<bool>> GenerateSiteMapAsync();
    }
}
