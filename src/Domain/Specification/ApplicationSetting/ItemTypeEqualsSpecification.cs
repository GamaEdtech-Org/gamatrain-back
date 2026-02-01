namespace GamaEdtech.Domain.Specification.ApplicationSetting
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;

    public sealed class ItemTypeEqualsSpecification(ItemType itemType) : SpecificationBase<SiteMap>
    {
        public override Expression<Func<SiteMap, bool>> Expression() => (t) => t.ItemType.Equals(itemType);
    }
}
