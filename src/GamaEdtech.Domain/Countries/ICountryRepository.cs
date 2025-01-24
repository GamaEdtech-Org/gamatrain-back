using GamaEdtech.Domain.Base;

namespace GamaEdtech.Domain.Countries;

public interface ICountryRepository
{
	public Task<Country?> GetBy(Id id);
	public Task<bool> ContainsCountrywithName(string name);
	public Task<bool> ContainsCountrywithCode(string code);
	public Task Add(Country country);
	public Task Remove(Country country);
}
