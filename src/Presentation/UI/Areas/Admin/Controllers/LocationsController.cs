namespace GamaEdtech.Backend.UI.Web.Areas.Admin.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using Farsica.Framework.Core;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAccess.Specification.Impl;

    using GamaEdtech.Backend.Data.Dto.Location;
    using GamaEdtech.Backend.Data.Entity;
    using GamaEdtech.Backend.Data.Enumeration;
    using GamaEdtech.Backend.Data.Specification.Location;
    using GamaEdtech.Backend.Data.ViewModel.Location;
    using GamaEdtech.Backend.Shared.Service;

    using Microsoft.AspNetCore.Mvc;

    [Farsica.Framework.DataAnnotation.Area(nameof(Admin), "Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ApiVersion("1.0")]
    //[Permission(Roles = [nameof(Role.Admin)])]
    public class LocationsController(Lazy<ILogger<LocationsController>> logger, Lazy<ILocationService> locationService)
        : ApiControllerBase<LocationsController>(logger)
    {
        #region Country

        [HttpGet("countries"), Produces<ApiResponse<ListDataSource<LocationsResponseViewModel>>>()]
        public async Task<IActionResult> GetCountries([NotNull, FromQuery] LocationsRequestViewModel request)
        {
            try
            {
                var result = await locationService.Value.GetLocationsAsync(new ListRequestDto<Location>
                {
                    PagingDto = request.PagingDto,
                    Specification = new LocationTypeEqualsSpecification(LocationType.Country),
                });
                return Ok(new ApiResponse<ListDataSource<LocationsResponseViewModel>>
                {
                    Errors = result.Errors,
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new LocationsResponseViewModel
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Code = t.Code,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ListDataSource<LocationsResponseViewModel>> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpGet("countries/{id:int}"), Produces<ApiResponse<LocationResponseViewModel>>()]
        public async Task<IActionResult> GetCountry([FromRoute] int id)
        {
            try
            {
                var result = await locationService.Value.GetLocationAsync(new IdEqualsSpecification<Location, int>(id));
                return Ok(new ApiResponse<LocationResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = result.Data is null ? null : new()
                    {
                        Id = result.Data.Id,
                        Title = result.Data.Title,
                        Code = result.Data.Code,
                        ParentId = result.Data.ParentId,
                        ParentTitle = result.Data.ParentTitle,
                        Latitude = result.Data.Latitude,
                        LocalTitle = result.Data.LocalTitle,
                        Longitude = result.Data.Longitude,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<LocationResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPost("countries"), Produces<ApiResponse<ManageLocationResponseViewModel>>()]
        public async Task<IActionResult> CreateCountry([NotNull] ManageLocationRequestViewModel request)
        {
            try
            {
                var result = await locationService.Value.ManageLocationAsync(new ManageLocationRequestDto
                {
                    Title = request.Title,
                    Code = request.Code,
                    ParentId = request.ParentId,
                    LocationType = LocationType.Country,
                    Latitude = request.Latitude,
                    LocalTitle = request.LocalTitle,
                    Longitude = request.Longitude,
                });
                return Ok(new ApiResponse<ManageLocationResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ManageLocationResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPut("countries/{id:int}"), Produces<ApiResponse<ManageLocationResponseViewModel>>()]
        public async Task<IActionResult> UpdateCountry([FromRoute] int id, [NotNull, FromBody] ManageLocationRequestViewModel request)
        {
            try
            {
                var result = await locationService.Value.ManageLocationAsync(new ManageLocationRequestDto
                {
                    Id = id,
                    Code = request.Code,
                    Title = request.Title,
                    ParentId = request.ParentId,
                    Latitude = request.Latitude,
                    LocalTitle = request.LocalTitle,
                    Longitude = request.Longitude,
                });
                return Ok(new ApiResponse<ManageLocationResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ManageLocationResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpDelete("countries/{id:int}"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult> RemoveCountry([FromRoute] int id)
        {
            try
            {
                var result = await locationService.Value.RemoveLocationAsync(new IdEqualsSpecification<Location, int>(id));
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

        #endregion

        #region State

        [HttpGet("states"), Produces<ApiResponse<ListDataSource<LocationsResponseViewModel>>>()]
        public async Task<IActionResult> GetStates([NotNull, FromQuery] LocationsRequestViewModel request)
        {
            try
            {
                var result = await locationService.Value.GetLocationsAsync(new ListRequestDto<Location>
                {
                    PagingDto = request.PagingDto,
                    Specification = new LocationTypeEqualsSpecification(LocationType.Country),
                });
                return Ok(new ApiResponse<ListDataSource<LocationsResponseViewModel>>
                {
                    Errors = result.Errors,
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new LocationsResponseViewModel
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Code = t.Code,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ListDataSource<LocationsResponseViewModel>> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpGet("states/{id:int}"), Produces<ApiResponse<LocationResponseViewModel>>()]
        public async Task<IActionResult> GetState([FromRoute] int id)
        {
            try
            {
                var result = await locationService.Value.GetLocationAsync(new IdEqualsSpecification<Location, int>(id));
                return Ok(new ApiResponse<LocationResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = result.Data is null ? null : new()
                    {
                        Id = result.Data.Id,
                        Title = result.Data.Title,
                        Code = result.Data.Code,
                        ParentId = result.Data.ParentId,
                        ParentTitle = result.Data.ParentTitle,
                        Latitude = result.Data.Latitude,
                        LocalTitle = result.Data.LocalTitle,
                        Longitude = result.Data.Longitude,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<LocationResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPost("states"), Produces<ApiResponse<ManageLocationResponseViewModel>>()]
        public async Task<IActionResult> CreateState([NotNull] ManageLocationRequestViewModel request)
        {
            try
            {
                var result = await locationService.Value.ManageLocationAsync(new ManageLocationRequestDto
                {
                    Title = request.Title,
                    Code = request.Code,
                    ParentId = request.ParentId,
                    LocationType = LocationType.State,
                    Latitude = request.Latitude,
                    LocalTitle = request.LocalTitle,
                    Longitude = request.Longitude,
                });
                return Ok(new ApiResponse<ManageLocationResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new() { Id = result.Data, }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ManageLocationResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPut("states/{id:int}"), Produces<ApiResponse<Void>>()]
        public async Task<IActionResult> UpdateState([FromRoute] int id, [NotNull, FromBody] ManageLocationRequestViewModel request)
        {
            try
            {
                var result = await locationService.Value.ManageLocationAsync(new ManageLocationRequestDto
                {
                    Id = id,
                    Code = request.Code,
                    Title = request.Title,
                    ParentId = request.ParentId,
                    Latitude = request.Latitude,
                    LocalTitle = request.LocalTitle,
                    Longitude = request.Longitude,
                });
                return Ok(new ApiResponse<ManageLocationResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ManageLocationResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpDelete("states/{id:int}"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult> RemoveState([FromRoute] int id)
        {
            try
            {
                var result = await locationService.Value.RemoveLocationAsync(new IdEqualsSpecification<Location, int>(id));
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

        #endregion

        #region City

        [HttpGet("cities"), Produces<ApiResponse<ListDataSource<LocationsResponseViewModel>>>()]
        public async Task<IActionResult> GetCities([NotNull, FromQuery] LocationsRequestViewModel request)
        {
            try
            {
                var result = await locationService.Value.GetLocationsAsync(new ListRequestDto<Location>
                {
                    PagingDto = request.PagingDto,
                    Specification = new LocationTypeEqualsSpecification(LocationType.Country),
                });
                return Ok(new ApiResponse<ListDataSource<LocationsResponseViewModel>>
                {
                    Errors = result.Errors,
                    Data = result.Data.List is null ? new() : new()
                    {
                        List = result.Data.List.Select(t => new LocationsResponseViewModel
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Code = t.Code,
                        }),
                        TotalRecordsCount = result.Data.TotalRecordsCount,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ListDataSource<LocationsResponseViewModel>> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpGet("cities/{id:int}"), Produces<ApiResponse<LocationResponseViewModel>>()]
        public async Task<IActionResult> GetCity([FromRoute] int id)
        {
            try
            {
                var result = await locationService.Value.GetLocationAsync(new IdEqualsSpecification<Location, int>(id));
                return Ok(new ApiResponse<LocationResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = result.Data is null ? null : new()
                    {
                        Id = result.Data.Id,
                        Title = result.Data.Title,
                        Code = result.Data.Code,
                        ParentId = result.Data.ParentId,
                        ParentTitle = result.Data.ParentTitle,
                        Latitude = result.Data.Latitude,
                        LocalTitle = result.Data.LocalTitle,
                        Longitude = result.Data.Longitude,
                    }
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<LocationResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPost("cities"), Produces<ApiResponse<ManageLocationResponseViewModel>>()]
        public async Task<IActionResult> CreateCity([NotNull] ManageLocationRequestViewModel request)
        {
            try
            {
                var result = await locationService.Value.ManageLocationAsync(new ManageLocationRequestDto
                {
                    Title = request.Title,
                    Code = request.Code,
                    ParentId = request.ParentId,
                    LocationType = LocationType.City,
                    Latitude = request.Latitude,
                    LocalTitle = request.LocalTitle,
                    Longitude = request.Longitude,
                });
                return Ok(new ApiResponse<ManageLocationResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ManageLocationResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpPut("cities/{id:int}"), Produces<ApiResponse<ManageLocationResponseViewModel>>()]
        public async Task<IActionResult> UpdateCity([FromRoute] int id, [NotNull, FromBody] ManageLocationRequestViewModel request)
        {
            try
            {
                var result = await locationService.Value.ManageLocationAsync(new ManageLocationRequestDto
                {
                    Id = id,
                    Code = request.Code,
                    Title = request.Title,
                    ParentId = request.ParentId,
                    Latitude = request.Latitude,
                    LocalTitle = request.LocalTitle,
                    Longitude = request.Longitude,
                });
                return Ok(new ApiResponse<ManageLocationResponseViewModel>
                {
                    Errors = result.Errors,
                    Data = new() { Id = result.Data, },
                });
            }
            catch (Exception exc)
            {
                Logger.Value.LogException(exc);

                return Ok(new ApiResponse<ManageLocationResponseViewModel> { Errors = [new() { Message = exc.Message }] });
            }
        }

        [HttpDelete("cities/{id:int}"), Produces<ApiResponse<bool>>()]
        public async Task<IActionResult> RemoveCity([FromRoute] int id)
        {
            try
            {
                var result = await locationService.Value.RemoveLocationAsync(new IdEqualsSpecification<Location, int>(id));
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

        #endregion
    }
}
