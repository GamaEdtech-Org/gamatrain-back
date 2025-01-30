namespace GamaEdtech.Backend.UI.Web.Api
{
    using System.Diagnostics.CodeAnalysis;

    using Asp.Versioning;

    using Farsica.Framework.Core;
    using Farsica.Framework.Data;

    using GamaEdtech.Backend.Data.Entity;
    using GamaEdtech.Backend.Data.Enumeration;
    using GamaEdtech.Backend.Data.Specification.Location;
    using GamaEdtech.Backend.Data.ViewModel.Location;
    using GamaEdtech.Backend.Shared.Service;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class LocationsController(Lazy<ILogger<LocationsController>> logger, Lazy<ILocationService> locationService) : ApiControllerBase<LocationsController>(logger)
    {
        [HttpGet("countries"), Produces(typeof(ApiResponse<ListDataSource<LocationsResponseViewModel>>))]
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

        [HttpGet("states"), Produces(typeof(ApiResponse<ListDataSource<LocationsResponseViewModel>>))]
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

        [HttpGet("cities"), Produces(typeof(ApiResponse<ListDataSource<LocationsResponseViewModel>>))]
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
    }
}
