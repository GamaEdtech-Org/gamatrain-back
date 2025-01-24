namespace GamaEdtech.Gateway.RestApi.Countries;

public class ContryInListDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Code { get; set; }
}

public class AddCountryDto
{
	public string Name { get; set; }
	public string Code { get; set; }
}

public class EditCountryInfoDto
{
	public string Name { get; set; }
	public string Code { get; set; }
}
