using ControlSystem.DAL.Interfaces;
using ControlSystem.Domain.Entities;
using ControlSystem.Domain.Enums;
using ControlSystem.Services.Implementations;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace ControlSystem.Tests.Services.UnitTests
{
    public class LinkServiceTests
    {
        [Fact]
        public async Task CreateLinkSuccsessful()
        {
            // Arrange
            var linkService = CreateLinkService();

            var ticketId = 1;

            var link = new Link
            {
                Id = 1,
                Name = "aboba",
                Source = "test"
            };

            // Act

            var result = await linkService.CreateLink(ticketId, link);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task DeleteLinkSuccessful()
        {
            // Arrange
            var linkService = CreateLinkService();

            var linkId = 1;

            // Act

            var result = await linkService.DeleteLink(linkId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task DeleteLinkFailed()
        {
            // Arrange
            var linkService = CreateLinkService();

            var linkId = 2;

            // Act

            var result = await linkService.DeleteLink(linkId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.LinkNotFound, result.StatusCode);
        }

        private LinkService CreateLinkService()
        {
            var linkRepoMock = new Mock<IRepository<Link>>();
            var loggerMock = new Mock<ILogger<LinkService>>();
            var ticketRepositoryMock = new Mock<IRepository<Ticket>>();

            ticketRepositoryMock.Setup(x => x.GetAll()).Returns(GetTickets().BuildMock());
            linkRepoMock.Setup(x => x.GetAll()).Returns(GetLinks().BuildMock());
            return new LinkService(
                loggerMock.Object,
                linkRepoMock.Object,
                ticketRepositoryMock.Object);
        }

        private IQueryable<Ticket> GetTickets()
        {
            return new List<Ticket>
            {
                new Ticket{ Id = 1 }
            }.AsQueryable();
        }

        private IQueryable<Link> GetLinks()
        {
            return new List<Link>
            {
                new Link
                {
                    Id = 1
                }
            }.AsQueryable();
        }
    }
}
