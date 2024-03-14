using ControlSystem.Domain.ViewModels;

namespace ControlSystem.Tests.Services.UnitTests
{
    public class UserAccountServiceTests
    {
        [Fact]
        public async Task RegisterUserSuccessful()
        {
            // Arrange

            UserAccountService userService = CreateService();

            var registerVM = new RegisterViewModel
            {
                Name = "Test",
                Email= "Test@ya.ru",
                Password = "12345",
                PasswordConfirm = "12345"
            };

            // Act

            var result = await userService.Register(registerVM);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Equal(result.Data.IsAuthenticated, true);

        }

        [Fact]
        public async Task RegisterUserFailed()
        {
            // Arrange

            UserAccountService userService = CreateService();

            var registerVM = new RegisterViewModel
            {
                Name = "TestLogin",
                Email= "Test@ya.ru",
                Password = "12345",
                PasswordConfirm = "12345"
            };

            // Act

            var result = await userService.Register(registerVM);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(StatusCode.UserAlreadyExists, result.StatusCode);
        }


        [Fact]
        public async Task LoginUserSuccessful()
        {
            // Arrange

            UserAccountService userService = CreateService();

            var loginVM = new LoginViewModel
            {
                Name = "TestLogin",
                Password = "12345"
            };

            // Act

            var result = await userService.Login(loginVM);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Equal(result.Data.IsAuthenticated, true);
        }

        [Fact]
        public async Task LoginUserFailed()
        {
            // Arrange

            UserAccountService userService = CreateService();

            var loginVM = new LoginViewModel
            {
                Name = "<invalid>",
                Password = "12345"
            };

            // Act

            var result = await userService.Login(loginVM);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(StatusCode.UserNotFound, result.StatusCode);
        }

        private UserAccountService CreateService()
        {
            var loggerMock = new Mock<ILogger<UserAccountService>>();
            var userRepositoryMock = new Mock<IRepository<UserAccount>>();
            userRepositoryMock.Setup(x => x.GetAll()).Returns(GetUsers().BuildMock());

            return new UserAccountService(
                loggerMock.Object,
                userRepositoryMock.Object
            );
        }

        private IQueryable<UserAccount> GetUsers()
        {
            return new List<UserAccount>()
            {
                new UserAccount
                {
                    Username = "TestLogin",
                    Password = "59-94-47-1a-bb-01-11-2a-fc-c1-81-59-f6-cc-74-b4-f5-11-b9-98-06-da-59-b3-ca-f5-a9-c1-73-ca-cf-c5"
                }
            }.AsQueryable();
        }
    }
}
