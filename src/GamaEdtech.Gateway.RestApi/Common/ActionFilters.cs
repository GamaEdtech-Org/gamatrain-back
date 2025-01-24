using Microsoft.AspNetCore.Mvc.Filters;

namespace GamaEdtech.Gateway.RestApi.Common;

public class PaginationTransformerAttribute : ActionFilterAttribute
{
	public int DefaultPage { get; set; } = 1;
	public int MaxPageSize { get; set; } = 50;
	public int DefaultPageSize { get; set; } = 10;

	public override void OnActionExecuting(ActionExecutingContext context)
	{
		var paginationDto = context.ExtractParameterOfType<PaginationDto>();

		if (paginationDto is null)
			return;

		Transform(paginationDto);

		context.ActionArguments[context.GetParameterNameOfType<PaginationDto>()!] = paginationDto;
	}

	private void Transform(PaginationDto paginationDto)
	{
		paginationDto.Page = GetOrTransformPage(paginationDto.Page);
		paginationDto.PageSize = GetOrTransformPageSize(paginationDto.PageSize);
	}

	private int GetOrTransformPage(int? page)
	{
		if (!page.HasValue)
			return DefaultPage;

		if (page.Value <= 0)
			return DefaultPage;

		return page.Value;
	}

	private int GetOrTransformPageSize(int? pageSize)
	{
		if (!pageSize.HasValue)
			return DefaultPageSize;

		if (pageSize.Value <= 0)
			return DefaultPageSize;

		if (pageSize.Value > 50)
			return MaxPageSize;

		return pageSize.Value;
	}
}

public class SortingTransformerAttribute : ActionFilterAttribute
{

	public string ValidSortKeys { get; set; } = "Name";
	public string DefaultSortKey { get; set; } = "Name";

	public override void OnActionExecuting(ActionExecutingContext context)
	{
		var sortingDto = context.ExtractParameterOfType<SortingDto>();

		if (sortingDto is null)
			return;

		Transform(sortingDto);

		context.ActionArguments[context.GetParameterNameOfType<SortingDto>()!] = sortingDto;
	}

	private void Transform(SortingDto sortingDto)
	{
		sortingDto.SortBy = GetOrTransformSortBy(sortingDto.SortBy);
		sortingDto.Order = GetOrTransformOrder(sortingDto.Order);
	}

	private string GetOrTransformSortBy(string? sortBy)
	{
		if (string.IsNullOrWhiteSpace(sortBy))
			return DefaultSortKey;

		var validSortKeys = ValidSortKeys.Split(",");

		if (!validSortKeys.Contains(sortBy.Trim()))
			return DefaultSortKey;

		return sortBy;
	}

	private string GetOrTransformOrder(string? order)
	{
		if (string.IsNullOrWhiteSpace(order))
			return "ASC";

		order = order.Trim().ToUpper();

		if (order != "ASC" && order != "DESC")
			return "ASC";

		return order;
	}
}

public static class ActionExecutingContextExtensions
{
	public static string? GetParameterNameOfType<T>(this ActionExecutingContext context)
	{
		var parameter = context.ActionDescriptor.Parameters
			.FirstOrDefault(p => p.ParameterType == typeof(T));

		if (parameter is null)
			return null;

		return parameter.Name;
	}

	public static T? ExtractParameterOfType<T>(this ActionExecutingContext context)
		where T : class
	{
		var parameterName = context.GetParameterNameOfType<T>();

		if (parameterName is null)
			return null;

		if (!context.ActionArguments.ContainsKey(parameterName))
			return null;

		var obj = context.ActionArguments[parameterName];

		if (obj is not T t)
			return null;

		return t;
	}
}
