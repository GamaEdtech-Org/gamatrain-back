using CSharpFunctionalExtensions;
using GamaEdtech.Domain.Base;

namespace GamaEdtech.Domain.Schools;

public class Address : ValueObject
{
	public string Description { get; }
	public Location Location { get; }
	public Id? StateId { get; }
	public Id CityId { get; set; }
	public string ZipCode { get; set; }

	protected Address() { }

	public Address(
		string description, 
		Location location, 
		Id? stateId, 
		Id cityId, 
		string zipCode)
	{
		Description = description;
		Location = location;
		StateId = stateId;
		CityId = cityId;
		ZipCode = zipCode;
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Description;
		yield return Location;
		yield return StateId;
		yield return CityId; 
		yield return ZipCode;
	}
}
