namespace GamaEdtech.Back.Gateway.Rest.Common;

public class PaginationDto
{
	public int? Page { get; set; }
	public int? PageSize { get; set; }
}

public class SortingDto
{
	public string? SortBy { get; set; }
	public string? Order { get; set; }
}