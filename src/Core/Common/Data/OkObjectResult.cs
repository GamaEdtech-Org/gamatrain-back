namespace GamaEdtech.Common.Data
{
    using Microsoft.AspNetCore.Mvc;

    public sealed class OkObjectResult<T>(object? value) : OkObjectResult(value), IActionResult<T>
    {
    }
}
