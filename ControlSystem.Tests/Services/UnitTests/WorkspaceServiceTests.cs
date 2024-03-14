using ControlSystem.Domain.ViewModels;

namespace ControlSystem.Tests.Services.UnitTests
{
    public class WorkspaceServiceTests
    {
        [Fact]
        public async Task CreateWorkspaceSuccessful()
        {
            // Arrange

            var workspaceService = CreateService();

            var username = "Test";
            var workspaceName = "Test";

            // Act

            var result = await workspaceService.CreateWorkspace(username, workspaceName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task CreateWorkspaceFailed()
        {
            // Arrange

            var workspaceService = CreateService();

            var username = "Test0963083";
            var workspaceName = "Test";

            // Act

            var result = await workspaceService.CreateWorkspace(username, workspaceName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.UserNotFound, result.StatusCode);
        }

        [Fact]
        public async Task DeleteWorkspaceSuccessful()
        {
            // Arrange

            var workspaceService = CreateService();
            var username = "Test";
            int id = 1;

            // Act
            var result = await workspaceService.DeleteWorkspace(username, id);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);
        }


        [Fact]
        public async Task DeleteWorkspaceFailed()
        {
            // Arrange

            var workspaceService = CreateService();
            var username = "Test";
            int id = 2;

            // Act
            var result = await workspaceService.DeleteWorkspace(username, id);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(StatusCode.WorkspaceNotFound, result.StatusCode);
        }

        [Fact]
        public async Task AddWorkspaceToUserSuccessful()
        {
            // Arrange

            var workspaceService = CreateService();
            var userId = GetUsers().First().Id;
            var workspaceId = GetWorkspaces().First().Id;

            // Act

            var result = await workspaceService.AddWorkspaceToUser(workspaceId, userId);

            // Assert

            Assert.Equal(StatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task AddWorkspaceToUserFailed()
        {
            // Arrange

            var workspaceService = CreateService();
            var userId = GetUsers().First().Id;
            var workspaceId = 187;

            // Act

            var result = await workspaceService.AddWorkspaceToUser(workspaceId, userId);

            // Assert

            Assert.Equal(StatusCode.WorkspaceNotFound, result.StatusCode);
        }

        [Fact]
        public async Task CreateBoardSuccessful()
        {
            // Arrange

            var workspaceService = CreateService();
            var workspaceId = GetWorkspaces().First().Id;
            var boardVM = new BoardViewModel { Name = "Test", ColorHex = "#FFF" };

            // Act

            var result = await workspaceService.CreateBoard(workspaceId, boardVM);

            // Assert

            Assert.Equal(StatusCode.OK, result.StatusCode);
        }


        [Fact]
        public async Task DeleteBoardSuccessful()
        {
            // Arrange

            var workspaceService = CreateService();
            var boardId = 1;

            // Act

            var result = await workspaceService.DeleteBoard(boardId);

            // Assert

            Assert.Equal(StatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task DeleteBoardFailed()
        {
            // Arrange

            var workspaceService = CreateService();
            var boardId = 217;

            // Act

            var result = await workspaceService.DeleteBoard(boardId);

            // Assert

            Assert.Equal(StatusCode.BoardNotFound, result.StatusCode);
        }

        [Fact]
        public async Task EditBoardSuccessful()
        {
            // Arrange

            var workspaceService = CreateService();
            var boardId = 1;
            var boardVM = new BoardViewModel { Name = "Test12356", ColorHex = "#FFF" };

            // Act

            var result = await workspaceService.EditBoard(boardId, boardVM);

            // Assert

            Assert.Equal(StatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task EditBoardFailed()
        {
            // Arrange

            var workspaceService = CreateService();
            var boardId = 232;
            var boardVM = new BoardViewModel { Name = "Test12356", ColorHex = "#FFF" };

            // Act

            var result = await workspaceService.EditBoard(boardId, boardVM);

            // Assert

            Assert.Equal(StatusCode.BoardNotFound, result.StatusCode);
        }

        [Fact]
        public async Task RenameWorkspaceSuccesful()
        {
            // Arrange

            var workspaceService = CreateService();
            int workspaceId = 1;
            string newName = "newName";

            // Act

            var result = await workspaceService.RenameWorkspace(workspaceId, newName);

            // Assert

            Assert.Equal(StatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task RenameWorkspaceFailed()
        {
            // Arrange

            var workspaceService = CreateService();
            int workspaceId = 109;
            string newName = "newName";

            // Act

            var result = await workspaceService.RenameWorkspace(workspaceId, newName);

            // Assert

            Assert.Equal(StatusCode.WorkspaceNotFound, result.StatusCode);
        }

        private WorkspaceService CreateService()
        {
            var loggerMock = new Mock<ILogger<WorkspaceService>>();

            var options = new DbContextOptionsBuilder<ControlSystemContext>().Options;
            var mockContext = new Mock<ControlSystemContext>(options);

            var userRepositoryMock = new Mock<UserAccountRepository>(mockContext.Object);
            userRepositoryMock.As<IRepository<UserAccount>>().Setup(x => x.GetAll()).Returns(GetUsers().BuildMock());

            var workspaceRepositoryMock = new Mock<WorkspaceRepository>(mockContext.Object);
            workspaceRepositoryMock.As<IRepository<Workspace>>().Setup(x => x.GetAll()).Returns(GetWorkspaces().BuildMock());

            var boardRepositoryMock = new Mock<IRepository<Board>>();
            boardRepositoryMock.Setup(x => x.GetAll()).Returns(GetBoards().BuildMock());

            return new WorkspaceService(
                loggerMock.Object,
                userRepositoryMock.Object,
                workspaceRepositoryMock.Object,
                boardRepositoryMock.Object
                );
        }

        private IQueryable<UserAccount> GetUsers()
        {
            return new List<UserAccount>()
            {
                new UserAccount
                {
                    Username = "Test",
                    Password = "59-94-47-1a-bb-01-11-2a-fc-c1-81-59-f6-cc-74-b4-f5-11-b9-98-06-da-59-b3-ca-f5-a9-c1-73-ca-cf-c5"
                }
            }.AsQueryable();
        }

        private IQueryable<Board> GetBoards()
        {
            return new List<Board>
            {
                new Board
                {
                    Id = 1,
                    Workspace = new Workspace()
                    {
                        Participants = GetUsers().ToList()
                    }
                }
            }.AsQueryable();
        }

        private IQueryable<Workspace> GetWorkspaces()
        {
            return new List<Workspace>
            {
                new Workspace
                {
                    Id = 1,
                    Boards = GetBoards().ToList(),
                    Name = "Test",
                    Participants = GetUsers().ToList()
                }

            }.AsQueryable();
        }
    }
}
