namespace GamaEdtech.Presentation.Api.Areas.Admin.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.Identity;
    using GamaEdtech.Data.Dto.File;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Presentation.ViewModel.File;

    using Microsoft.AspNetCore.Mvc;

    [Common.DataAnnotation.Area(nameof(Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    [Permission(Roles = [nameof(Role.Admin)])]
    public class FilesController(Lazy<ILogger<FilesController>> logger, Lazy<IFileService> fileService) : ApiControllerBase<FilesController>(logger)
    {
        [HttpPost, Produces(typeof(ApiResponse<CreateFileResponseViewModel>))]
        public async Task<IActionResult> CreateFile([NotNull, FromForm] CreateFileRequestViewModel request)
        {
            try
            {
                var result = await fileService.Value.CreateFileAsync(new CreateFileRequestDto
                {
                    File = request.File,
                });
                return Ok(new ApiResponse<CreateFileResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new()
                    {
                        FileId = result.Data?.FileId,
                        PreviewId1 = result.Data?.PreviewId1,
                        PreviewId2 = result.Data?.PreviewId2,
                        PreviewId3 = result.Data?.PreviewId3,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<CreateFileResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }
    }
}
