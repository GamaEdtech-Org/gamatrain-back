namespace GamaEdtech.Test.Presentation.Api.Areas.Admin.Controllers
{
    using GamaEdtech.Application.Interface;
    using GamaEdtech.Presentation.Api.Areas.Admin.Controllers;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Xunit;

    public class LocationsControllerUnitTest : TestBase
    {
        [Fact]
        public async Task ValidateTestAsync()
        {
            var logger = Services.Value!.GetRequiredService<Lazy<ILogger<LocationsController>>>();
            var locationService = Services.Value!.GetRequiredService<Lazy<ILocationService>>();
            var controller = new LocationsController(logger, locationService);
            var response = await controller.GetCities(new()
            {
                PagingDto = new(),
            });

            Assert.NotNull(response);
        }
    }
}
