using CSharpFunctionalExtensions;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace GamaEdtech.Domain.Schools;

public class Location : ValueObject
{
	public Point Geography { get; }

	public double Latitude => Geography.Y; // Latitude is Y in NetTopologySuite
	public double Longitude => Geography.X; // Longitude is X in NetTopologySuite

	private Location() { }

	public Location(double latitude, double longitude)
	{
		var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
		Geography = geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Geography;
	}
}