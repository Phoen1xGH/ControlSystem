namespace ControlSystem.Tests.Services.UnitTests
{
    public class BpmnServiceTests
    {
        [Fact]
        public async Task CreateDiagramSuccessful()
        {
            // Arrange

            var chartService = CreateService();
            var username = "Test";
            var chart = new Chart { Id = 2, Title = "Test2", XmlData = "<xml>" };

            // Act

            var result = await chartService.SaveBPMNToDB(username, chart);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);

        }

        [Fact]
        public async Task CreateDiagramFailed()
        {
            // Arrange

            var chartService = CreateService();
            var username = "Test8621";
            var chart = new Chart { Id = 2, Title = "Test2", XmlData = "<xml>" };

            // Act

            var result = await chartService.SaveBPMNToDB(username, chart);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.UserNotFound, result.StatusCode);

        }


        [Fact]
        public async Task DeleteChartSuccessful()
        {
            // Arrange

            var chartService = CreateService();
            int chartId = 1;

            // Act

            var result = await chartService.DeleteChart(chartId);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task DeleteChartFailed()
        {
            // Arrange

            var chartService = CreateService();
            int chartId = 17;

            // Act

            var result = await chartService.DeleteChart(chartId);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(StatusCode.ChartNotFound, result.StatusCode);
        }

        [Fact]
        public void GenerateProcessSuccessful()
        {
            // Arrange

            var chartService = CreateService();
            var filesDirectory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent + @"\BpmnDiagrams");
            var xml = File.ReadAllText(filesDirectory + @"\correctDiagram.json");

            // Act

            var result = chartService.GenerateProcess(xml);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);
        }

        [Fact]
        public void GenerateProcessFailed()
        {
            // Arrange

            var chartService = CreateService();
            var filesDirectory = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent + @"\BpmnDiagrams");
            var xml = File.ReadAllText(filesDirectory + @"\unCorrectDiagram.json");

            // Act

            var result = chartService.GenerateProcess(xml);

            // Assert

            Assert.NotNull(result);
            Assert.Equal(StatusCode.InternalServerError, result.StatusCode);
        }

        public BPMNGenerateService CreateService()
        {
            var loggerMock = new Mock<ILogger<BPMNGenerateService>>();

            var options = new DbContextOptionsBuilder<ControlSystemContext>().Options;
            var mockContext = new Mock<ControlSystemContext>(options);

            var userRepositoryMock = new Mock<UserAccountRepository>(mockContext.Object);
            userRepositoryMock.As<IRepository<UserAccount>>().Setup(x => x.GetAll()).Returns(GetUsers().BuildMock());

            var chartRepositoryMock = new Mock<IRepository<Chart>>();
            chartRepositoryMock.Setup(x => x.GetAll()).Returns(GetCharts().BuildMock());

            return new BPMNGenerateService(
                loggerMock.Object,
                userRepositoryMock.Object,
                chartRepositoryMock.Object
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

        private IQueryable<Chart> GetCharts()
        {
            return new List<Chart>
            {
                new Chart
                {
                    Id = 1,
                    Title = "Test",
                    XmlData = ""
                }
            }.AsQueryable();
        }
    }
}
