namespace GamaEdtech.Gateway.RestApi.States;

public class FilterStatesDto
{
	public int? CountryId { get; set; }
}

public class StateInListDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Code { get; set; }
	public int CountryId { get; set; }
}

public class AddStateDto
{
	public int CountryId { get; set; }
	public string Name { get; set; }
	public string Code { get; set; }
}

public class EditStateInfoDto
{
	public string Name { get; set; }
	public string Code { get; set; }
}

public class MoveStateToAnotherDto
{
	public int CountryId { get; set; }
}