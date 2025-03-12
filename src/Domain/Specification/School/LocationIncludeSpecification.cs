namespace GamaEdtech.Domain.Specification.School
{
    using System.Linq.Expressions;

    using GamaEdtech.Common.DataAccess.Specification;
    using GamaEdtech.Domain.Entity;

    using NetTopologySuite;
    using NetTopologySuite.Geometries;

    public sealed class LocationIncludeSpecification(double latitude, double longitude, double radius) : SpecificationBase<School>
    {
        public Point Point
        {
            get
            {
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
                return geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
            }
        }

        public override Expression<Func<School, bool>> Expression() => (t) => t.Coordinates != null && t.Coordinates.Distance(Point) < radius;
    }
}
