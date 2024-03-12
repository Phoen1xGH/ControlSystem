using ControlSystem.DAL;
using ControlSystem.DAL.Interfaces;
using ControlSystem.DAL.Repositories;
using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Enums;
using ControlSystem.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ControlSystem.Tests.Services.UnitTests
{
    public class BoardServiceTests
    {
        [Fact]
        public async Task CreateTicketSuccessful()
        {
            // Arrange

            BoardService boardService = CreateService();

            var username = "Daniel";
            var title = "Новая задача";
            var boardId = 1;

            // Act

            var result = await boardService.CreateTicket(username, title, boardId);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task DeleteTicketSuccessful()
        {
            // Arrange

            BoardService boardService = CreateService();

            int ticketId = 1;

            // Act

            var result = await boardService.DeleteTicket(ticketId);

            // Assert
            Assert.Equal(StatusCode.OK, result.StatusCode);
        }


        [Fact]
        public async Task DeleteTicketFailed()
        {
            // Arrange

            BoardService boardService = CreateService();

            int ticketId = 2;

            // Act

            var result = await boardService.DeleteTicket(ticketId);

            // Assert
            Assert.Equal(StatusCode.TicketNotFound, result.StatusCode);
        }


        private BoardService CreateService()
        {
            var loggerMock = new Mock<ILogger<BoardService>>();
            var userRepositoryMock = new Mock<IRepository<UserAccount>>();

            var commentRepositoryMock = new Mock<IRepository<Comment>>();
            var fileRepositoryMock = new Mock<IRepository<FileAttachment>>();

            userRepositoryMock.Setup(x => x.GetAll()).Returns(GetUsers().BuildMock());

            var options = new DbContextOptionsBuilder<ControlSystemContext>()
                .Options;

            var mockContext = new Mock<ControlSystemContext>(options);

            var boardRepositoryMock = new Mock<BoardRepository>(mockContext.Object);
            boardRepositoryMock.As<IRepository<Board>>().Setup(x => x.GetAll()).Returns(GetBoards().BuildMock());

            var ticketRepositoryMock = new Mock<TicketRepository>(mockContext.Object);
            ticketRepositoryMock.As<IRepository<Ticket>>().Setup(x => x.GetAll()).Returns(GetTickets().BuildMock());

            return new BoardService(
                loggerMock.Object,
                userRepositoryMock.Object,
                boardRepositoryMock.Object,
                ticketRepositoryMock.Object,
                commentRepositoryMock.Object,
                fileRepositoryMock.Object
            );
        }


        private IQueryable<UserAccount> GetUsers()
        {
            return new List<UserAccount>
            {
                new UserAccount()
                {
                    Username = "Daniel",

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

        private IQueryable<Ticket> GetTickets()
        {
            return new List<Ticket>
            {
                new Ticket{ Id = 1 }
            }.AsQueryable();
        }
    }
}
