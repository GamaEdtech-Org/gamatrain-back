namespace GamaEdtech.Test.Api.Controllers
{

    using System;

    using GamaEdtech.Application.Interface;
    using GamaEdtech.Common.Data;
    using GamaEdtech.Data.Dto.Identity;
    using GamaEdtech.Domain.Entity.Identity;
    using GamaEdtech.Presentation.Api.Controllers;
    using GamaEdtech.Presentation.ViewModel.Identity;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Moq;

    using Xunit;

    using static GamaEdtech.Common.Core.Constants;

    public class IdentitiesControllerTests
    {
        private readonly Mock<ILogger<IdentitiesController>> loggerMock;
        private readonly Mock<IIdentityService> identityServiceMock;
        private readonly Mock<UserManager<ApplicationUser>> userManagerMock;
        private readonly Mock<IHttpClientFactory> httpClientFactoryMock;
        private readonly IdentitiesController controller;

        public IdentitiesControllerTests()
        {
            loggerMock = new Mock<ILogger<IdentitiesController>>();
            identityServiceMock = new Mock<IIdentityService>();
            userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                Options.Create(new IdentityOptions()),
                Mock.Of<IPasswordHasher<ApplicationUser>>(),
                Array.Empty<IUserValidator<ApplicationUser>>(),
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<ApplicationUser>>>()
            );
            httpClientFactoryMock = new Mock<IHttpClientFactory>();

            controller = new IdentitiesController(
                new Lazy<ILogger<IdentitiesController>>(() => loggerMock.Object),
                new Lazy<IIdentityService>(() => identityServiceMock.Object),
                new Lazy<UserManager<ApplicationUser>>(() => userManagerMock.Object),
                new Lazy<IHttpClientFactory>(() => httpClientFactoryMock.Object)
            );
        }

        [Fact]
        public async Task RegisterValidRequestReturnsOkResult()
        {
            // Arrange
            var request = new RegistrationRequestViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            _ = identityServiceMock.Setup(x => x.RegisterAsync(It.IsAny<RegistrationRequestDto>()))
                .ReturnsAsync(new ResultData<bool>(OperationResult.Succeeded)
                {
                    Data = true
                });

            // Act
            var result = await controller.Register(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult<Common.Data.Void>>(result);
            var response = Assert.IsType<ApiResponse<Common.Data.Void>>(okResult.Value);
            Assert.True(response.Succeeded);
            Assert.Null(response.Errors);

            identityServiceMock.Verify(
                x => x.RegisterAsync(It.Is<RegistrationRequestDto>(dto =>
                    dto.Email == request.Email &&
                    dto.Password == request.Password &&
                    dto.Username == request.Email)),
                Times.Once);
        }

        [Fact]
        public async Task RegisterServiceThrowsExceptionReturnsErrorResponse()
        {
            // Arrange
            var request = new RegistrationRequestViewModel
            {
                Email = "test@example.com",
                Password = "Password123!"
            };

            var expectedException = new Exception("Registration failed");
            _ = identityServiceMock.Setup(x => x.RegisterAsync(It.IsAny<RegistrationRequestDto>()))
                .ThrowsAsync(expectedException);

            // Act
            var result = await controller.Register(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult<Common.Data.Void>>(result);
            var response = Assert.IsType<ApiResponse<Common.Data.Void>>(okResult.Value);
            Assert.False(response.Succeeded);
            Assert.NotNull(response.Errors);
            Assert.Contains(response.Errors, error => error.Message == expectedException.Message);
        }

        [Fact]
        public async Task RegisterDuplicateUsernameReturnsNotValid()
        {
            // Arrange
            var request = new RegistrationRequestViewModel { Email = "duplicate@example.com", Password = "Password123!" };

            _ = identityServiceMock.Setup(x => x.RegisterAsync(It.IsAny<RegistrationRequestDto>()))
                .ReturnsAsync(new ResultData<bool>(OperationResult.NotValid)
                {
                    Errors = new[] { new Error { Message = "Duplicate username" } }
                });

            // Act
            var result = await controller.Register(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult<Common.Data.Void>>(result);
            var response = Assert.IsType<ApiResponse<Common.Data.Void>>(okResult.Value);

            Assert.False(response.Succeeded);
            Assert.NotNull(response.Errors);
            Assert.Contains(response.Errors, e => e.Message!.Contains("Duplicate", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task RegisterWithEmptyEmailOrPasswordReturnsError()
        {
            // Arrange
            var request = new RegistrationRequestViewModel { Email = "", Password = "" };

            _ = identityServiceMock.Setup(x => x.RegisterAsync(It.IsAny<RegistrationRequestDto>()))
                .ReturnsAsync(new ResultData<bool>(OperationResult.NotValid)
                {
                    Errors = new[] { new Error { Message = "Email and password are required" } }
                });

            // Act
            var result = await controller.Register(request);

            // // Assert
            var okResult = Assert.IsType<OkObjectResult<Common.Data.Void>>(result);
            var response = Assert.IsType<ApiResponse<Common.Data.Void>>(okResult.Value);

            Assert.False(response.Succeeded);
            Assert.NotNull(response.Errors);
            Assert.Contains(response.Errors, e => e.Message == "Email and password are required");
        }

    }
}
