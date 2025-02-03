namespace GamaEdtech.Backend.UI.Web.Api
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using Farsica.Framework.Core;
    using Farsica.Framework.Data;

    using GamaEdtech.Backend.Data.Entity;
    using GamaEdtech.Backend.Data.Specification.Subject;
    using GamaEdtech.Backend.Data.ViewModel.Subject;
    using GamaEdtech.Backend.Shared.Service;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SubjectsController(Lazy<ILogger<SubjectsController>> logger, Lazy<ISubjectService> boardService)
        : ApiControllerBase<SubjectsController>(logger)
    {
        [HttpGet, Produces<ApiResponse<ListDataSource<SubjectsResponseViewModel>>>()]
        public async Task<IActionResult> GetSubjects([NotNull, FromQuery] SubjectsRequestViewModel request)
        {
            try
            {
                var result = await boardService.Value.GetSubjectsAsync(new ListRequestDto<Subject>
                {
                    PagingDto = request.PagingDto,
                    Specification = request.GradeId.HasValue ? new GradeIdEqualsSpecification(request.GradeId.Value) : null,
                });
                return Ok(new ApiResponse<ListDataSource<SubjectsResponseViewModel>>
                {
                    Errors = result.Errors,
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new SubjectsResponseViewModel
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Order = t.Order,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ListDataSource<SubjectsResponseViewModel>> { Errors = [new() { Message = exc.Message }] });
            }
        }
    }
}
