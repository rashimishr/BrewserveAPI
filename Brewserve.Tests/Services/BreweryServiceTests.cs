using AutoMapper;
using BrewServe.Core.Payloads;
using BrewServe.Core.Services;
using BrewServe.Data.Interfaces;
using BrewServe.Data.Models;
using Moq;

namespace BrewServe.Tests.Services
{
    [TestFixture]
    public class BreweryServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private BreweryService _breweryService;

        [SetUp]
        public void SetUp()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _breweryService = new BreweryService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetBreweriesAsync_ShouldReturnBreweries()
        {
            // Arrange
            var breweries = new List<Brewery> { new Brewery { Id = 1, Name = "Brewery1" } };
            var breweryResponses = new List<BreweryResponse> { new BreweryResponse { Id = 1, Name = "Brewery1" } };

            _unitOfWorkMock.Setup(u => u.Breweries.GetAllAsync()).ReturnsAsync(breweries);
            _mapperMock.Setup(m => m.Map<IEnumerable<BreweryResponse>>(breweries)).Returns(breweryResponses);

            // Act
            var result = await _breweryService.GetBreweriesAsync();

            // Assert
            Assert.AreEqual(breweryResponses, result);
        }

        [Test]
        public async Task GetBreweryByIdAsync_ShouldReturnBrewery()
        {
            // Arrange
            var brewery = new Brewery { Id = 1, Name = "Brewery1" };
            var breweryResponse = new BreweryResponse { Id = 1, Name = "Brewery1" };

            _unitOfWorkMock.Setup(u => u.Breweries.GetByIdAsync(1)).ReturnsAsync(brewery);
            _mapperMock.Setup(m => m.Map<BreweryResponse>(brewery)).Returns(breweryResponse);

            // Act
            var result = await _breweryService.GetBreweryByIdAsync(1);

            // Assert
            Assert.AreEqual(breweryResponse, result);
        }

        [Test]
        public async Task AddBreweryAsync_ShouldReturnNullIfBreweryExists()
        {
            // Arrange
            var breweryRequest = new BreweryRequest { Name = "Brewery1" };
            var breweries = new List<Brewery> { new Brewery { Name = "Brewery1" } };

            _unitOfWorkMock.Setup(u => u.Breweries.GetAllAsync()).ReturnsAsync(breweries);

            // Act
            var result = await _breweryService.AddBreweryAsync(breweryRequest);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddBreweryAsync_ShouldAddBrewery()
        {
            // Arrange
            var breweryRequest = new BreweryRequest { Name = "Brewery1" };
            var brewery = new Brewery { Name = "Brewery1" };
            var breweryResponse = new BreweryResponse { Name = "Brewery1" };

            _unitOfWorkMock.Setup(u => u.Breweries.GetAllAsync()).ReturnsAsync(new List<Brewery>());
            _mapperMock.Setup(m => m.Map<Brewery>(breweryRequest)).Returns(brewery);
            _unitOfWorkMock.Setup(u => u.Breweries.AddAsync(brewery)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.FromResult(1));
            _mapperMock.Setup(m => m.Map<BreweryResponse>(brewery)).Returns(breweryResponse);

            // Act
            var result = await _breweryService.AddBreweryAsync(breweryRequest);

            // Assert
            Assert.AreEqual(breweryResponse, result);
        }

        [Test]
        public async Task UpdateBreweryAsync_ShouldUpdateBrewery()
        {
            // Arrange
            var breweryRequest = new BreweryRequest { Id = 1, Name = "Brewery1" };
            var brewery = new Brewery { Id = 1, Name = "Brewery1" };
            var breweryResponse = new BreweryResponse { Id = 1, Name = "Brewery1" };
            
            _mapperMock.Setup(m => m.Map<Brewery>(breweryRequest)).Returns(brewery);
            _unitOfWorkMock.Setup(u => u.Breweries.UpdateAsync(brewery)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.FromResult(1));
            _mapperMock.Setup(m => m.Map<BreweryResponse>(brewery)).Returns(breweryResponse);

            // Act
            var result = await _breweryService.UpdateBreweryAsync(breweryRequest);

            // Assert
            Assert.AreEqual(breweryResponse, result);
        }

        [Test]
        public async Task GetBreweryBeerLinkAsync_ShouldReturnBreweryBeerLinks()
        {
            // Arrange
            var breweries = new List<Brewery> { new Brewery { Id = 1, Name = "Brewery1" } };
            var breweryBeerLinkResponses = new List<BreweryBeerLinkResponse> { new BreweryBeerLinkResponse { Id = 1, Name = "Brewery1" } };

            _unitOfWorkMock.Setup(u => u.BreweryBeersLinks.GetAssociatedBreweryBeersAsync()).ReturnsAsync(breweries);
            _mapperMock.Setup(m => m.Map<IEnumerable<BreweryBeerLinkResponse>>(breweries)).Returns(breweryBeerLinkResponses);

            // Act
            var result = await _breweryService.GetBreweryBeerLinkAsync();

            // Assert
            Assert.AreEqual(breweryBeerLinkResponses, result);
        }

        [Test]
        public async Task GetBreweryBeerLinkByBreweryIdAsync_ShouldReturnBreweryBeerLink()
        {
            // Arrange
            var brewery = new Brewery { Id = 1, Name = "Brewery1" };
            var breweryBeerLinkResponse = new BreweryBeerLinkResponse { Id = 1, Name = "Brewery1" };

            _unitOfWorkMock.Setup(u => u.BreweryBeersLinks.GetAssociatedBreweryBeersByBreweryIdAsync(1)).ReturnsAsync(brewery);
            _mapperMock.Setup(m => m.Map<BreweryBeerLinkResponse>(brewery)).Returns(breweryBeerLinkResponse);

            // Act
            var result = await _breweryService.GetBreweryBeerLinkByBreweryIdAsync(1);

            // Assert
            Assert.AreEqual(breweryBeerLinkResponse, result);
        }

        [Test]
        public async Task AddBreweryBeerLinkAsync_ShouldReturnNullIfBreweryOrBeerNotFound()
        {
            // Arrange
            var breweryBeerLinkRequest = new BreweryBeerLinkRequest { BreweryId = 1, BeerId = 1 };

            _unitOfWorkMock.Setup(u => u.Breweries.GetByIdAsync(1)).ReturnsAsync((Brewery)null);
            _unitOfWorkMock.Setup(u => u.Beers.GetByIdAsync(1)).ReturnsAsync((Beer)null);

            // Act
            var result = await _breweryService.AddBreweryBeerLinkAsync(breweryBeerLinkRequest);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task AddBreweryBeerLinkAsync_ShouldAddBreweryBeerLink()
        {
            // Arrange
            var breweryBeerLinkRequest = new BreweryBeerLinkRequest { BreweryId = 1, BeerId = 1 };
            var brewery = new Brewery { Id = 1, Name = "Brewery1" };
            var beer = new Beer { Id = 1, Name = "Beer1", PercentageAlcoholByVolume = 5.0m };
            var breweryBeerLink = new BreweryBeerLink { BreweryId = 1, BeerId = 1 };
            var breweryResponse = new BreweryResponse { Id = 1, Name = "Brewery1" };

            _unitOfWorkMock.Setup(u => u.Breweries.GetByIdAsync(1)).ReturnsAsync(brewery);
            _unitOfWorkMock.Setup(u => u.Beers.GetByIdAsync(1)).ReturnsAsync(beer);
            _mapperMock.Setup(m => m.Map<BreweryBeerLink>(breweryBeerLinkRequest)).Returns(breweryBeerLink);
            _unitOfWorkMock.Setup(u => u.BreweryBeersLinks.AddAsync(breweryBeerLink)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.FromResult(1));
            _mapperMock.Setup(m => m.Map<BreweryResponse>(breweryBeerLink)).Returns(breweryResponse);

            // Act
            var result = await _breweryService.AddBreweryBeerLinkAsync(breweryBeerLinkRequest);

            // Assert
            Assert.AreEqual(breweryResponse, result);
        }

    }
}
