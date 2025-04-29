namespace GamaEdtech.Presentation.Api.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;

    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Specification.Subject;
    using GamaEdtech.Presentation.ViewModel.Subject;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SubjectsController(Lazy<ILogger<SubjectsController>> logger, Lazy<ISubjectService> boardService)
        : ApiControllerBase<SubjectsController>(logger)
    {
        [HttpGet, Produces<ApiResponse<ListDataSource<SubjectsResponseViewModel>>>()]
        public async Task<IActionResult<ListDataSource<SubjectsResponseViewModel>>> GetSubjects([NotNull, FromQuery] SubjectsRequestViewModel request)
        {
            try
            {
                var result = await boardService.Value.GetSubjectsAsync(new ListRequestDto<Subject>
                {
                    PagingDto = request.PagingDto,
                    Specification = request.GradeId.HasValue ? new GradeIdEqualsSpecification(request.GradeId.Value) : null,
                });
                return Ok<ListDataSource<SubjectsResponseViewModel>>(new(result.Errors)
                {
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

                return Ok<ListDataSource<SubjectsResponseViewModel>>(new() { Errors = [new() { Message = exc.Message }] });
            }
        }
    }
}
