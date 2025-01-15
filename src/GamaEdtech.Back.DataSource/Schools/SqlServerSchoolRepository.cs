using GamaEdtech.Back.DataSource.Utils;
using GamaEdtech.Back.Domain.Base;
using GamaEdtech.Back.Domain.Schools;
using Microsoft.EntityFrameworkCore;

namespace GamaEdtech.Back.DataSource.Schools;

public class SqlServerSchoolRepository : ISchoolRepository
{
	private readonly GamaEdtechDbContext _dbContext;

	public SqlServerSchoolRepository(GamaEdtechDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<School?> GetBy(Id id)
	{
		return await _dbContext.Schools.FindAsync(id);
	}

	public async Task<bool> ContainsSchoolInCityWith(Id id)
	{
		return await _dbContext.Schools
			.Where(x => x.Address.CityId == id)
			.AnyAsync();
	}

	public async Task<IReadOnlyList<School>> Find(string? namePattern = null)
	{
		IQueryable<School> schools = _dbContext.Schools;

		if (namePattern is not null)
			schools = schools.Where(
				x => x.Name.InEnglish.Contains(namePattern) ||
				x.Name.InLocalLanguage.Contains(namePattern));

		return await schools.ToListAsync();
	}

	public async Task<IReadOnlyList<School>> FindByLocation(
		Location location, double radiusInKm = 0)
	{
		var geometryFactory = 
			NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
		var searchLocation = geometryFactory.CreatePoint(
			new NetTopologySuite.Geometries.Coordinate(location.Longitude, location.Latitude));

		return await _dbContext.Schools
			.Where(s => s.Address.Location.Geography.Distance(searchLocation) <= radiusInKm * 1000)
			.ToListAsync();
	}

	public async Task Add(School school)
	{
		await _dbContext.Schools.AddAsync(school);
	}

	public async Task Remove(School school)
	{
		_dbContext.Schools.Remove(school);
	}
}
