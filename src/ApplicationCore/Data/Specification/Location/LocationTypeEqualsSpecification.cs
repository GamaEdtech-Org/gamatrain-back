namespace GamaEdtech.Backend.Data.Specification.Location
{
    using System.Linq.Expressions;

    using Farsica.Framework.DataAccess.Specification;

    using GamaEdtech.Backend.Data.Entity;
    using GamaEdtech.Backend.Data.Enumeration;

    public sealed class LocationTypeEqualsSpecification(LocationType locationType) : SpecificationBase<Location>
    {
        public override Expression<Func<Location, bool>> Expression() => (t) => t.LocationType == locationType;
    }
}
