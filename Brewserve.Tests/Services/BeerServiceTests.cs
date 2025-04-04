using AutoMapper;
using BrewServe.Core.Interfaces;
using BrewServe.Core.Payloads;
using BrewServe.Core.Services;
using BrewServe.Core.Strategies;
using BrewServe.Data.Interfaces;
using BrewServe.Data.Models;
using BrewServe.Data.Repositories;
using Moq;

namespace BrewServe.Tests.Services
{
    [TestFixture]
    public class BeerServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private BeerService _beerService;
        private Mock<IBeerSearchStrategy> _strategyMock;
       
        [SetUp]
        public void SetUp()        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _strategyMock = new Mock<IBeerSearchStrategy>();
            _beerService = new BeerService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetBeersAsync_ShouldReturnBeers()
        {
            // Arrange
            var beers = new List<Beer> { new Beer { Id = 1, Name = "Beer1", PercentageAlcoholByVolume = 5.0m } };
            var beerResponses = new List<BeerResponse> { new BeerResponse { Id = 1, Name = "Beer1", PercentageAlcoholByVolume = 5.0m } };

            _unitOfWorkMock.Setup(u => u.Beers.GetAllAsync()).ReturnsAsync(beers);
            _mapperMock.Setup(m => m.Map<IEnumerable<BeerResponse>>(beers)).Returns(beerResponses);

            // Act
            var result = await _beerService.GetBeersAsync();

            // Assert
            Assert.AreEqual(beerResponses, result);
        }

        [Test]
        public async Task GetBeerByIdAsync_ShouldReturnBeer()
        {
            // Arrange
            var beer = new Beer { Id = 1, Name = "Beer1", PercentageAlcoholByVolume = 5.0m };
            var beerResponse = new BeerResponse { Id = 1, Name = "Beer1", PercentageAlcoholByVolume = 5.0m };

            _unitOfWorkMock.Setup(u => u.Beers.GetByIdAsync(1)).ReturnsAsync(beer);
            _mapperMock.Setup(m => m.Map<BeerResponse>(beer)).Returns(beerResponse);

            // Act
            var result = await _beerService.GetBeerByIdAsync(1);

            // Assert
            Assert.AreEqual(beerResponse, result);
        }

        [Test]
        public async Task AddBeerAsync_ShouldAddBeer()
        {
            // Arrange
            var beerRequest = new BeerRequest { Name = "Beer1", PercentageAlcoholByVolume = 5.0m };
            var beer = new Beer { Name = "Beer1", PercentageAlcoholByVolume = 5.0m };
            var beerResponse = new BeerResponse { Name = "Beer1", PercentageAlcoholByVolume = 5.0m };

            _mapperMock.Setup(m => m.Map<Beer>(beerRequest)).Returns(beer);
            _unitOfWorkMock.Setup(u => u.Beers.AddAsync(beer)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.FromResult(1));
            _mapperMock.Setup(m => m.Map<BeerResponse>(beer)).Returns(beerResponse);

            // Act
            var result = await _beerService.AddBeerAsync(beerRequest);

            // Assert
            Assert.AreEqual(beerResponse, result);
        }

        [Test]
        public async Task UpdateBeerAsync_ShouldUpdateBeer()
        {
            // Arrange
            var beerRequest = new BeerRequest { Id = 1, Name = "Beer1", PercentageAlcoholByVolume = 5.0m };
            var beer = new Beer { Id = 1, Name = "Beer1", PercentageAlcoholByVolume = 5.0m };
            var beerResponse = new BeerResponse { Id = 1, Name = "Beer1", PercentageAlcoholByVolume = 5.0m };

            _mapperMock.Setup(m => m.Map<Beer>(beerRequest)).Returns(beer);
            _unitOfWorkMock.Setup(u => u.Beers.UpdateAsync(beer)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.FromResult(1));
            _mapperMock.Setup(m => m.Map<BeerResponse>(beer)).Returns(beerResponse);

            // Act
            var result = await _beerService.UpdateBeerAsync(beerRequest);

            // Assert
            Assert.AreEqual(beerResponse, result);
        }

    }
}
