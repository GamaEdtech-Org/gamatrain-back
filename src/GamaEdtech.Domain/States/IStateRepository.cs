using GamaEdtech.Domain.Base;

namespace GamaEdtech.Domain.States;

public interface IStateRepository
{
	public Task<State?> GetBy(Id id);
	public Task<bool> ContainsStateWithNameInCountry(string name, Id countryId);
	public Task<bool> ContainsStateWithCodeInCountry(string code, Id countryId);
	public Task<bool> ContainsStateInCountryWith(Id id);
	public Task Add(State state);
	public Task Remove(State state);
}
