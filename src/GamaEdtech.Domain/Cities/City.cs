using GamaEdtech.Domain.Base;

namespace GamaEdtech.Domain.Cities;

public class City : AggregateRoot
{
	public string Name { get; private set; }
	public Id? StateId { get; private set; }
	public Id CountryId { get; private set; }

	public City(string name, Id? stateId, Id countryId)
	{
		Name = name;
		StateId = stateId;
		CountryId = countryId;
	}

	public bool IsPartOfAState => StateId is not null;

	public void EditInfo(string name)
	{
		Name = name;
	}
}
