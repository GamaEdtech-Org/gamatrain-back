#pragma warning disable xUnit1041 // Fixture arguments to test classes must have fixture sources
#pragma warning disable S125 // Sections of code should not be commented out
#pragma warning disable CS9113 // Parameter is unread.
namespace GamaEdtech.Backend.Test.DomainService
{
    using GamaEdtech.Backend.Shared.Service;

    public class UserServiceUnitTest(IIdentityService identityService)
    {

        /*
                [Fact]
                public async Task GetUsersAsync()
                {
                    var response = await identityService.GetUsersAsync();

                    Assert.Equal(Farsica.Framework.Core.Constants.OperationResult.Succeeded, response.OperationResult);
                }
                */
    }
}
#pragma warning restore CS9113 // Parameter is unread.
#pragma warning restore S125 // Sections of code should not be commented out
#pragma warning restore xUnit1041 // Fixture arguments to test classes must have fixture sources
