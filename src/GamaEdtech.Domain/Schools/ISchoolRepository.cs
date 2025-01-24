using GamaEdtech.Domain.Base;

namespace GamaEdtech.Domain.Schools;

public interface ISchoolRepository
{
	public Task<School?> GetBy(Id id);
	public Task<bool> ContainsSchoolInCityWith(Id id);
	public Task<IReadOnlyList<School>> Find(string? namePattern = null);
	public Task<IReadOnlyList<School>> FindByLocation(
		Location location, double radiusInKm = 0);

	public Task Add(School school);
	public Task Remove(School school);
}
