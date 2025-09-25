namespace GamaEdtech.Test.Application
{
    using GamaEdtech.Application.Interface;

    using Microsoft.Extensions.DependencyInjection;

    using Xunit;

    using static GamaEdtech.Common.Core.Constants;

    public class IdentityServiceUnitTest : TestBase
    {
        [Fact]
        public async Task GetUsersAsync()
        {
            var service = Services.Value!.GetRequiredService<IIdentityService>();
            var response = await service.GetUsersAsync();

            Assert.Equal(OperationResult.Succeeded, response.OperationResult);
        }
    }
}
