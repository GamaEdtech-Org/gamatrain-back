namespace GamaEdtech.Gateway.RestApi.Controllers;

public class RegisterSchoolDto
{
	public SchoolNameDto Name { get; set; }
	public int Type { get; set; }
	public AddressDto Address { get; set; }
}

public class SchoolInListDto
{
	public int Id { get; set; }
	public SchoolNameDto Name { get; set; }
	public int Type { get; set; }
}

public class SearchByLocationDto
{
	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public double RadiusInKm { get; set; }
}


public class SchoolNameDto
{
	public string InEnglish { get; set; }
	public string InLocalLanguage { get; set; }
}

public class AddressDto
{
	public string Description { get; set; }
	public LocationDto Location { get; set; }
	public int? StateId { get; set; }
	public int CityId { get; set; }
	public string ZipCode { get; set; }

}

public class LocationDto
{
	public double Latitude { get; set; }
	public double Longitude { get; set; }
}



