using GamaEdtech.Domain.Base;
using GamaEdtech.Domain.Cities;

namespace GamaEdtech.Domain.Countries;

public class Country : AggregateRoot
{
	public string Name { get; private set; }
	public string Code { get; private set; }

	public Country(string name, string code)
	{
		Name = name;
		Code = code;
	}

	public void EditInfo(string name, string code)
	{
		Name = name;
		Code = code;
	}

	public City CreateCityWith(string name)
	{
		return new City(name, null, Id);
	}
}
