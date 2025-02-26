namespace GamaEdtech.Domain.Specification.School
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    using NetTopologySuite;

    public sealed class LocationIncludeSpecification(double latitude, double longitude, double radius) : SpecificationBase<School>
    {
        public override Expression<Func<School, bool>> Expression()
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            var location = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(longitude, latitude));

            return (t) => t.Location != null && t.Location.Distance(location) < radius;
        }
    }
}
