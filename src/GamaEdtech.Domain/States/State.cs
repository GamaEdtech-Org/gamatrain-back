using GamaEdtech.Domain.Base;
using GamaEdtech.Domain.Cities;
using GamaEdtech.Domain.Countries;

namespace GamaEdtech.Domain.States;

public class State : AggregateRoot
{
	public string Name { get; private set; }
	public string Code { get; private set; }
	public Id CountryId { get; private set; }

	public State(string name, string code, Id countryId)
	{
		Name = name;
		Code = code;
		CountryId = countryId;
	}

	public void EditInfo(string name, string code)
	{
		Name = name; 
		Code = code;
	}

	public void MoveTo(Country country)
	{
		CountryId = country.Id;
	}

	public City CreateCityWith(string name)
	{
		return new City(name, Id, CountryId);
	}
}
