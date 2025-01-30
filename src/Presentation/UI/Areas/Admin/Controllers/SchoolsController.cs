namespace GamaEdtech.Backend.UI.Web.Areas.Admin.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using Farsica.Framework.Core;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Specification.Impl;

    using GamaEdtech.Backend.Data.Dto.School;
    using GamaEdtech.Backend.Data.Entity;
    using GamaEdtech.Backend.Data.ViewModel.School;
    using GamaEdtech.Backend.Shared.Service;

    using Microsoft.AspNetCore.Mvc;

    [Farsica.Framework.DataAnnotation.Area(nameof(Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    //[Permission(Roles = [nameof(Role.Admin)])]
    public class SchoolsController(Lazy<ILogger<SchoolsController>> logger, Lazy<ISchoolService> schoolService)
        : ApiControllerBase<SchoolsController>(logger)
    {
        [HttpGet, Produces<ApiResponse<ListDataSource<SchoolsResponseViewModel>>>()]
        public async Task<IActionResult> GetSchools([NotNull, FromQuery] SchoolsRequestViewModel request)
        {
            try
            {
                var result = await schoolService.Value.GetSchoolsAsync(new ListRequestDto<School>
                {
                    PagingDto = request.PagingDto,
                });
                return Ok(new ApiResponse<ListDataSource<SchoolsResponseViewModel>>
                {
                    Errors = result.Errors,
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new SchoolsResponseViewModel
                        {
                            Id = t.Id,
                            Name = t.Name,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ListDataSource<SchoolsResponseViewModel>> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpGet("{id:int}"), Produces<ApiResponse<SchoolResponseViewModel>>()]
        public async Task<IActionResult> GetSchool([FromRoute] int id)
        {
            try
            {
                var result = await schoolService.Value.GetSchoolAsync(new IdEqualsSpecification<School, int>(id));
                return Ok(new ApiResponse<SchoolResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = result.Data is null ? null : new()
                    {
                        Id = result.Data.Id,
                        Address = result.Data.Address,
                        Name = result.Data.Name,
                        SchoolType = result.Data.SchoolType,
                        StateId = result.Data.StateId,
                        StateTitle = result.Data.StateTitle,
                        ZipCode = result.Data.ZipCode,
                        Latitude = result.Data.Latitude,
                        Longitude = result.Data.Longitude,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<SchoolResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPost, Produces<ApiResponse<ManageSchoolResponseViewModel>>()]
        public async Task<IActionResult> CreateSchool([NotNull] ManageSchoolRequestViewModel request)
        {
            try
            {
                var result = await schoolService.Value.ManageSchoolAsync(new ManageSchoolRequestDto
                {
                    Address = request.Address!,
                    Name = request.Name!,
                    SchoolType = request.SchoolType!,
                    StateId = request.StateId!.Value,
                    ZipCode = request.ZipCode!,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                });
                return Ok(new ApiResponse<ManageSchoolResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ManageSchoolResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPut("{id:int}"), Produces<ApiResponse<ManageSchoolResponseViewModel>>()]
        public async Task<IActionResult> UpdateSchool([FromRoute] int id, [NotNull, FromBody] ManageSchoolRequestViewModel request)
        {
            try
            {
                var result = await schoolService.Value.ManageSchoolAsync(new ManageSchoolRequestDto
                {
                    Id = id,
                    Address = request.Address!,
                    Name = request.Name!,
                    SchoolType = request.SchoolType!,
                    StateId = request.StateId!.Value,
                    ZipCode = request.ZipCode!,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                });
                return Ok(new ApiResponse<ManageSchoolResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ManageSchoolResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpDelete("{id:int}"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult> RemoveSchool([FromRoute] int id)
        {
            try
            {
                var result = await schoolService.Value.RemoveSchoolAsync(new IdEqualsSpecification<School, int>(id));
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
