using AutoMapper;
using BrewServe.Core.Payloads;
using BrewServe.Core.Services;
using BrewServe.Data.Interfaces;
using BrewServe.Data.Models;
using Moq;

namespace BrewServe.Tests.Services
{
    [TestFixture]
    public class BarServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private BarService _barService;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _barService = new BarService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetBarsAsync_ShouldReturnBars()
        {
            // Arrange
            var bars = new List<Bar> { new Bar { Id = 1, Name = "Bar1", Address = "Address1" } };
            var barResponses = new List<BarResponse> { new BarResponse { Id = 1, Name = "Bar1", Address = "Address1" } };

            _unitOfWorkMock.Setup(u => u.Bars.GetAllAsync()).ReturnsAsync(bars);
            _mapperMock.Setup(m => m.Map<IEnumerable<BarResponse>>(bars)).Returns(barResponses);

            // Act
            var result = await _barService.GetBarsAsync();

            // Assert
            Assert.AreEqual(barResponses, result);
        }

        [Test]
        public async Task GetBarByIdAsync_ShouldReturnBar()
        {
            // Arrange
            var bar = new Bar { Id = 1, Name = "Bar1", Address = "Address1" };
            var barResponse = new BarResponse { Id = 1, Name = "Bar1", Address = "Address1" };

            _unitOfWorkMock.Setup(u => u.Bars.GetByIdAsync(1)).ReturnsAsync(bar);
            _mapperMock.Setup(m => m.Map<BarResponse>(bar)).Returns(barResponse);

            // Act
            var result = await _barService.GetBarByIdAsync(1);

            // Assert
            Assert.AreEqual(barResponse, result);
        }

        [Test]
        public async Task AddBarAsync_ShouldReturnNullIfBarExists()
        {
            // Arrange
            var barRequest = new BarRequest { Name = "Bar1", Address = "Address1" };
            var bars = new List<Bar> { new Bar { Name = "Bar1", Address = "Address1" } };

            _unitOfWorkMock.Setup(u => u.Bars.GetAllAsync()).ReturnsAsync(bars);

            // Act
            var result = await _barService.AddBarAsync(barRequest);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddBarAsync_ShouldAddBar()
        {
            // Arrange
            var barRequest = new BarRequest { Name = "Bar1", Address = "Address1" };
            var bar = new Bar { Name = "Bar1", Address = "Address1" };
            var barResponse = new BarResponse { Name = "Bar1", Address = "Address1" };

            _unitOfWorkMock.Setup(u => u.Bars.GetAllAsync()).ReturnsAsync(new List<Bar>());
            _mapperMock.Setup(m => m.Map<Bar>(barRequest)).Returns(bar);
            _unitOfWorkMock.Setup(u => u.Bars.AddAsync(bar)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.FromResult(1));
            _mapperMock.Setup(m => m.Map<BarResponse>(bar)).Returns(barResponse);

            // Act
            var result = await _barService.AddBarAsync(barRequest);

            // Assert
            Assert.AreEqual(barResponse, result);
        }

        [Test]
        public async Task UpdateBarAsync_ShouldUpdateBar()
        {
            // Arrange
            var barRequest = new BarRequest { Id = 1, Name = "Bar1", Address = "Address1" };
            var bar = new Bar { Id = 1, Name = "Bar1", Address = "Address1" };
            var barResponse = new BarResponse { Id = 1, Name = "Bar1", Address = "Address1" };

            _mapperMock.Setup(m => m.Map<Bar>(barRequest)).Returns(bar);
            _unitOfWorkMock.Setup(u => u.Bars.UpdateAsync(bar)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.FromResult(1));
            _mapperMock.Setup(m => m.Map<BarResponse>(bar)).Returns(barResponse);

            // Act
            var result = await _barService.UpdateBarAsync(barRequest);

            // Assert
            Assert.AreEqual(barResponse, result);
        }

        [Test]
        public async Task GetBarBeerLinkAsync_ShouldReturnBarBeerLinks()
        {
            // Arrange
            var bars = new List<Bar> { new Bar { Id = 1, Name = "Bar1", Address = "Address1" } };
            var barBeerLinkResponses = new List<BarBeerLinkResponse> { new BarBeerLinkResponse { Id = 1, Name = "Bar1", Address = "Address1" } };

            _unitOfWorkMock.Setup(u => u.BarBeersLinks.GetAssociatedBarBeersAsync()).ReturnsAsync(bars);
            _mapperMock.Setup(m => m.Map<IEnumerable<BarBeerLinkResponse>>(bars)).Returns(barBeerLinkResponses);

            // Act
            var result = await _barService.GetBarBeerLinkAsync();

            // Assert
            Assert.AreEqual(barBeerLinkResponses, result);
        }

        [Test]
        public async Task GetBarBeerLinkByBarIdAsync_ShouldReturnBarBeerLink()
        {
            // Arrange
            var bar = new Bar { Id = 1, Name = "Bar1", Address = "Address1" };
            var barBeerLinkResponse = new BarBeerLinkResponse { Id = 1, Name = "Bar1", Address = "Address1" };

            _unitOfWorkMock.Setup(u => u.BarBeersLinks.GetAssociatedBarBeersByBarIdAsync(1)).ReturnsAsync(bar);
            _mapperMock.Setup(m => m.Map<BarBeerLinkResponse>(bar)).Returns(barBeerLinkResponse);

            // Act
            var result = await _barService.GetBarBeerLinkByBarIdAsync(1);

            // Assert
            Assert.AreEqual(barBeerLinkResponse, result);
        }

        [Test]
        public async Task AddBarBeerLinkAsync_ShouldReturnNullIfBarOrBeerNotFound()
        {
            // Arrange
            var barBeerLinkRequest = new BarBeerLinkRequest { BarId = 1, BeerId = 1 };

            _unitOfWorkMock.Setup(u => u.Bars.GetByIdAsync(1)).ReturnsAsync((Bar)null);
            _unitOfWorkMock.Setup(u => u.Beers.GetByIdAsync(1)).ReturnsAsync((Beer)null);

            // Act
            var result = await _barService.AddBarBeerLinkAsync(barBeerLinkRequest);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddBarBeerLinkAsync_ShouldAddBarBeerLink()
        {
            // Arrange
            var barBeerLinkRequest = new BarBeerLinkRequest { BarId = 1, BeerId = 1 };
            var bar = new Bar { Id = 1, Name = "Bar1", Address = "Address1" };
            var beer = new Beer { Id = 1, Name = "Beer1", PercentageAlcoholByVolume = 5.0m };
            var barBeerLink = new BarBeerLink { BarId = 1, BeerId = 1 };
            var barResponse = new BarResponse { Id = 1, Name = "Bar1", Address = "Address1" };

            _unitOfWorkMock.Setup(u => u.Bars.GetByIdAsync(1)).ReturnsAsync(bar);
            _unitOfWorkMock.Setup(u => u.Beers.GetByIdAsync(1)).ReturnsAsync(beer);
            _mapperMock.Setup(m => m.Map<BarBeerLink>(barBeerLinkRequest)).Returns(barBeerLink);
            _unitOfWorkMock.Setup(u => u.BarBeersLinks.AddAsync(barBeerLink)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.FromResult(1));
            _mapperMock.Setup(m => m.Map<BarResponse>(barBeerLink)).Returns(barResponse);

            // Act
            var result = await _barService.AddBarBeerLinkAsync(barBeerLinkRequest);

            // Assert
            Assert.AreEqual(barResponse, result);
        }

    }
}
