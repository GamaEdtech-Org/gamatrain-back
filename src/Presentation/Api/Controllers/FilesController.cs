namespace GamaEdtech.Presentation.Api.Controllers
{
    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class FilesController(Lazy<ILogger<FilesController>> logger, Lazy<IFileService> fileService)
        : ApiControllerBase<FilesController>(logger)
    {
        [HttpGet("{id}"), Produces<ApiResponse<string>>()]
        public IActionResult GetFile([FromRoute] string id)
        {
            try
            {
                var result = fileService.Value.GetFileUri(id);
                return result.OperationResult is Constants.OperationResult.Succeeded
                    ? Redirect(result.Data!.ToString())
                    : Ok(new ApiResponse<string?>(result.Errors));
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<string?>(new Error { Message = exc.Message }));
            }
        }
    }
}
