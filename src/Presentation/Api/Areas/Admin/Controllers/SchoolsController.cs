namespace GamaEdtech.Presentation.Api.Areas.Admin.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Core;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Common.DataAccess.Specification.Impl;
    using GamaEdtech.Common.Identity;

    using GamaEdtech.Data.Dto.School;
    using GamaEdtech.Domain.Entity;
    using GamaEdtech.Domain.Enumeration;
    using GamaEdtech.Domain.Specification.School;
    using GamaEdtech.Presentation.ViewModel.School;

    using Microsoft.AspNetCore.Mvc;

    using NetTopologySuite;
    using NetTopologySuite.Geometries;

    [Common.DataAnnotation.Area(nameof(Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    [Permission(Roles = [nameof(Role.Admin)])]
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
                            LocalName = t.LocalName,
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
                        LocalAddress = result.Data.LocalAddress,
                        Name = result.Data.Name,
                        LocalName = result.Data.LocalName,
                        SchoolType = result.Data.SchoolType,
                        StateId = result.Data.StateId,
                        StateTitle = result.Data.StateTitle,
                        ZipCode = result.Data.ZipCode,
                        Latitude = result.Data.Location?.Y,
                        Longitude = result.Data.Location?.X,
                        Facilities = result.Data.Facilities,
                        WebSite = result.Data.WebSite,
                        Email = result.Data.Email,
                        CityId = result.Data.CityId,
                        CityTitle = result.Data.CityTitle,
                        CountryId = result.Data.CountryId,
                        CountryTitle = result.Data.CountryTitle,
                        FaxNumber = result.Data.FaxNumber,
                        PhoneNumber = result.Data.PhoneNumber,
                        Quarter = result.Data.Quarter,
                        OsmId = result.Data.OsmId,
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
                Point? location = null;
                if (request.Latitude.HasValue && request.Longitude.HasValue)
                {
                    var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
                    location = geometryFactory.CreatePoint(new Coordinate(request.Longitude.Value, request.Latitude.Value));
                }

                var result = await schoolService.Value.ManageSchoolAsync(new ManageSchoolRequestDto
                {
                    Address = request.Address,
                    Name = request.Name,
                    LocalName = request.LocalName,
                    SchoolType = request.SchoolType!,
                    StateId = request.StateId,
                    ZipCode = request.ZipCode,
                    Location = location,
                    Quarter = request.Quarter,
                    PhoneNumber = request.PhoneNumber,
                    FaxNumber = request.FaxNumber,
                    Email = request.Email,
                    CountryId = request.CountryId,
                    CityId = request.CityId,
                    Facilities = request.Facilities,
                    LocalAddress = request.LocalAddress,
                    WebSite = request.WebSite,
                    OsmId = request.OsmId,
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
                Point? location = null;
                if (request.Latitude.HasValue && request.Longitude.HasValue)
                {
                    var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
                    location = geometryFactory.CreatePoint(new Coordinate(request.Longitude.Value, request.Latitude.Value));
                }
                var result = await schoolService.Value.ManageSchoolAsync(new ManageSchoolRequestDto
                {
                    Id = id,
                    Address = request.Address,
                    Name = request.Name,
                    LocalName = request.LocalName,
                    SchoolType = request.SchoolType!,
                    StateId = request.StateId,
                    ZipCode = request.ZipCode,
                    Location = location,
                    WebSite = request.WebSite,
                    LocalAddress = request.LocalAddress,
                    Facilities = request.Facilities,
                    CityId = request.CityId,
                    CountryId = request.CountryId,
                    Email = request.Email,
                    FaxNumber = request.FaxNumber,
                    PhoneNumber = request.PhoneNumber,
                    Quarter = request.Quarter,
                    OsmId = request.OsmId,
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

        [HttpGet("comments/pending"), Produces<ApiResponse<ListDataSource<SchoolCommentsResponseViewModel>>>()]
        public async Task<IActionResult<ListDataSource<SchoolCommentsResponseViewModel>>> GetPendingSchoolComments([NotNull, FromQuery] SchoolCommentsRequestViewModel request)
        {
            try
            {
                var result = await schoolService.Value.GetSchoolCommentsAsync(new()
                {
                    PagingDto = request.PagingDto,
                    Specification = new StatusEqualsSpecification<SchoolComment>(Status.Draft),
                });
                return Ok(new ApiResponse<ListDataSource<SchoolCommentsResponseViewModel>>(result.Errors)
                {
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new SchoolCommentsResponseViewModel
                        {
                            Id = t.Id,
                            Comment = t.Comment,
                            AverageRate = t.AverageRate,
                            CreationDate = t.CreationDate,
                            CreationUser = t.CreationUser,
                            CreationUserAvatar = t.CreationUserAvatar,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ListDataSource<SchoolCommentsResponseViewModel>>(new Error { Message = exc.Message }));
            }
        }

        [HttpPatch("commonts/{commentId:long}/confirm"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult> ConfirmSchoolComment([FromRoute] long commentId)
        {
            try
            {
                var result = await schoolService.Value.ConfirmSchoolCommentAsync(new IdEqualsSpecification<SchoolComment, long>(commentId));
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

        [HttpPatch("commonts/{commentId:long}/reject"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult> RejectSchoolComment([FromRoute] long commentId)
        {
            try
            {
                var result = await schoolService.Value.RejectSchoolCommentAsync(new IdEqualsSpecification<SchoolComment, long>(commentId));
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

        [HttpGet("images/pending"), Produces<ApiResponse<ListDataSource<SchoolImagesResponseViewModel>>>()]
        public async Task<IActionResult<ListDataSource<SchoolImagesResponseViewModel>>> GetPendingSchoolImages([NotNull, FromQuery] SchoolImagesRequestViewModel request)
        {
            try
            {
                var result = await schoolService.Value.GetSchoolImagesAsync(new()
                {
                    PagingDto = request.PagingDto,
                    Specification = new StatusEqualsSpecification<SchoolImage>(Status.Draft),
                });
                return Ok(new ApiResponse<ListDataSource<SchoolImagesResponseViewModel>>(result.Errors)
                {
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new SchoolImagesResponseViewModel
                        {
                            Id = t.Id,
                            CreationDate = t.CreationDate,
                            CreationUser = t.CreationUser,
                            CreationUserAvatar = t.CreationUserAvatar,
                            FileId = t.FileId,
                            FileType = t.FileType,
                            SchoolId = t.SchoolId,
                            SchoolName = t.SchoolName,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ListDataSource<SchoolImagesResponseViewModel>>(new Error { Message = exc.Message }));
            }
        }

        [HttpPatch("images/{imageId:long}/confirm"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult> ConfirmSchoolImage([FromRoute] long imageId)
        {
            try
            {
                var result = await schoolService.Value.ConfirmSchoolImageAsync(new IdEqualsSpecification<SchoolImage, long>(imageId));
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

        [HttpPatch("images/{imageId:long}/reject"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult> RejectSchoolImage([FromRoute] long imageId)
        {
            try
            {
                var result = await schoolService.Value.RejectSchoolImageAsync(new IdEqualsSpecification<SchoolImage, long>(imageId));
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
