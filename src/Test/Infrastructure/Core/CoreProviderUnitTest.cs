namespace GamaEdtech.Test.Infrastructure.Core
{
    using GamaEdtech.Infrastructure.Interface;
    using GamaEdtech.Presentation.Api;

    using Microsoft.Extensions.DependencyInjection;

    using Xunit;

    using static GamaEdtech.Common.Core.Constants;

    public class CoreProviderUnitTest
    {
        [Fact]
        public async Task ValidateTestAsync()
        {
            var service = Startup.Services.Value!.GetRequiredService<ICoreProvider>();
            var response = await service.ValidateTestAsync(new()
            {
                TestId = 1,
                SubmissionId = 1,
                UserId = 1,
            });

            Assert.Equal(OperationResult.Succeeded, response.OperationResult);
        }
    }
}
