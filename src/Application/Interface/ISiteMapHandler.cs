namespace GamaEdtech.Application.Interface
{
    using System.Diagnostics.CodeAnalysis;

    using GamaEdtech.Common.DataAccess.UnitOfWork;
    using GamaEdtech.Common.DataAnnotation;
    using GamaEdtech.Data.Dto.SiteMap;
    using GamaEdtech.Domain.Enumeration;

    [Injectable]
    public interface ISiteMapHandler
    {
        ItemType ItemType { get; }
        IQueryable<SiteMapItemDto> GetSiteMapData([NotNull] IUnitOfWork uow);
    }
}
