using Microsoft.AspNetCore.Mvc;

namespace GamaEdtech.Gateway.RestApi.Utils;

public class ModelStateValidator
{
	public static IActionResult Validate(ActionContext context)
	{
		List<string> errors = new List<string>();

		foreach (var pair in context.ModelState)
			errors.Add(string.Join("||", pair.Value.Errors
				.Where(x => !x.ErrorMessage.StartsWith("The"))
				.Select(x => x.ErrorMessage)));


		return new BadRequestObjectResult(Envelope.Error(string.Join("||", errors)));
	}
}
