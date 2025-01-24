using GamaEdtech.Domain.Base;

namespace GamaEdtech.Domain.Cities;

public interface ICityRepository
{
	public Task<City?> GetBy(Id id);
	public Task<bool> ContainsCityWithNameInCountry(string name, Id countryId);
	public Task<bool> ContainsCityWithNameInState(string name, Id stateId);
	public Task<bool> ContainsCityInCountryWith(Id id);
	public Task<bool> ContainsCityInStateWith(Id id);
	public Task Add(City city);
	public Task Remove(City city);
}
