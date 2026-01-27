namespace GamaEdtech.Application.Interface
{
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Data.Dto.SiteMap;

    [Injectable]
    public interface ISiteMapHandler
    {
        Task<ResultData<List<SiteMapItemDto>>> GetSiteMapDataAsync();
    }
}
