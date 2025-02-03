namespace GamaEdtech.Backend.UI.Web.Areas.Admin.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using Farsica.Framework.Core;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Specification.Impl;

    using GamaEdtech.Backend.Data.Dto.Grade;
    using GamaEdtech.Backend.Data.Entity;
    using GamaEdtech.Backend.Data.Specification.Grade;
    using GamaEdtech.Backend.Data.ViewModel.Grade;
    using GamaEdtech.Backend.Shared.Service;

    using Microsoft.AspNetCore.Mvc;

    [Farsica.Framework.DataAnnotation.Area(nameof(Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    //[Permission(Roles = [nameof(Role.Admin)])]
    public class GradesController(Lazy<ILogger<GradesController>> logger, Lazy<IGradeService> gradeService)
        : ApiControllerBase<GradesController>(logger)
    {
        [HttpGet, Produces<ApiResponse<ListDataSource<GradesResponseViewModel>>>()]
        public async Task<IActionResult> GetGrades([NotNull, FromQuery] GradesRequestViewModel request)
        {
            try
            {
                var result = await gradeService.Value.GetGradesAsync(new ListRequestDto<Grade>
                {
                    PagingDto = request.PagingDto,
                    Specification = request.BoardId.HasValue ? new BoardIdEqualsSpecification(request.BoardId.Value) : null,
                });
                return Ok(new ApiResponse<ListDataSource<GradesResponseViewModel>>
                {
                    Errors = result.Errors,
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new GradesResponseViewModel
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Icon = t.Icon,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ListDataSource<GradesResponseViewModel>> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpGet("{id:int}"), Produces<ApiResponse<GradeResponseViewModel>>()]
        public async Task<IActionResult> GetGrade([FromRoute] int id)
        {
            try
            {
                var result = await gradeService.Value.GetGradeAsync(new IdEqualsSpecification<Grade, int>(id));
                return Ok(new ApiResponse<GradeResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = result.Data is null ? null : new()
                    {
                        Id = result.Data.Id,
                        Title = result.Data.Title,
                        Description = result.Data.Description,
                        Icon = result.Data.Icon,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<GradeResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPost, Produces<ApiResponse<ManageGradeResponseViewModel>>()]
        public async Task<IActionResult> CreateGrade([NotNull] ManageGradeRequestViewModel request)
        {
            try
            {
                var result = await gradeService.Value.ManageGradeAsync(new ManageGradeRequestDto
                {
                    Title = request.Title,
                    Description = request.Description,
                    Icon = request.Icon,
                });
                return Ok(new ApiResponse<ManageGradeResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ManageGradeResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPut("{id:int}"), Produces<ApiResponse<ManageGradeResponseViewModel>>()]
        public async Task<IActionResult> UpdateGrade([FromRoute] int id, [NotNull, FromBody] ManageGradeRequestViewModel request)
        {
            try
            {
                var result = await gradeService.Value.ManageGradeAsync(new ManageGradeRequestDto
                {
                    Id = id,
                    Title = request.Title,
                    Description = request.Description,
                    Icon = request.Icon,
                });
                return Ok(new ApiResponse<ManageGradeResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ManageGradeResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpDelete("{id:int}"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult> RemoveGrade([FromRoute] int id)
        {
            try
            {
                var result = await gradeService.Value.RemoveGradeAsync(new IdEqualsSpecification<Grade, int>(id));
                return Ok(new ApiResponse<bool>
                {
                    Errors = result.Errors,
                    Data = result.Data
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<bool> { Errors = [new() { Message = exc.Message }] });
            }
        }
    }
}
