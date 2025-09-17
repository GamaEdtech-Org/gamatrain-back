namespace GamaEdtech.Test.Application
{
    using GamaEdtech.Application.Interface;
    using GamaEdtech.Presentation.Api;

    using Microsoft.Extensions.DependencyInjection;

    using Xunit;

    using static GamaEdtech.Common.Core.Constants;

    public class IdentityServiceUnitTest
    {
        [Fact]
        public async Task GetUsersAsync()
        {
            var service = Startup.Services.Value!.GetRequiredService<IIdentityService>();
            var response = await service.GetUsersAsync();

            Assert.Equal(OperationResult.Succeeded, response.OperationResult);
        }
    }
}
