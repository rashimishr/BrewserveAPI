using BrewServe.API.Controllers;
using BrewServe.Core.Constants;
using BrewServe.Core.Interfaces;
using BrewServe.Core.Payloads;
using BrewServe.Core.Strategies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BrewServe.Tests.Controllers
{
    [TestFixture]
    public class BeerControllerTests
    {
        private Mock<IBeerService> _mockBeerService;
        private Mock<IBeerSearchStrategy> _mockStrategy;
        private Mock<ILogger<BeerController>> _mockLogger;
        private BeerController _controller;

        [SetUp]
        public void Setup()
        {
            _mockBeerService = new Mock<IBeerService>();
            _mockStrategy = new Mock<IBeerSearchStrategy>();
            _mockLogger = new Mock<ILogger<BeerController>>();
            _controller = new BeerController(_mockBeerService.Object, _mockStrategy.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetBeers_ReturnsOkResult_WithBeers()
        {
            // Arrange
            var beers = new List<BeerResponse>
            {
                new BeerResponse { Id = 1, Name = "Beer1", PercentageAlcoholByVolume = 5.0m },
                new BeerResponse { Id = 2, Name = "Beer2", PercentageAlcoholByVolume = 6.0m }
            };
            _mockStrategy.Setup(strategy => strategy.FilterAsync()).ReturnsAsync(beers);

            // Act
            var result = await _controller.GetBeers(4.0m, 7.0m);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ApiResponse<IEnumerable<BeerResponse>>;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(beers, response.Data);
        }

        [Test]
        public async Task GetBeers_ReturnsBadRequest_WhenInvalidAlcoholContent()
        {
            // Act
            var result = await _controller.GetBeers(-1.0m, 7.0m);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            var response = badRequestResult.Value as ApiResponse<List<BeerResponse>>;
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.AreEqual("Alcohol content must be greater than 0", response.Message);
        }

        [Test]
        public async Task GetBeers_ReturnsOkResult_WithErrorMessage_WhenNoBeersFound()
        {
            // Arrange
            _mockStrategy.Setup(strategy => strategy.FilterAsync()).ReturnsAsync(new List<BeerResponse>());

            // Act
            var result = await _controller.GetBeers(4.0m, 7.0m);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ApiResponse<BeerResponse>;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual("No record found", response.Message);
        }

        [Test]
        public async Task GetBeerByIdAsync_ReturnsOkResult_WithBeer()
        {
            // Arrange
            var beer = new BeerResponse { Id = 1, Name = "Beer1", PercentageAlcoholByVolume = 5.0m };
            _mockBeerService.Setup(service => service.GetBeerByIdAsync(1)).ReturnsAsync(beer);

            // Act
            var result = await _controller.GetBeerByIdAsync(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ApiResponse<BeerResponse>;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(beer, response.Data);
        }

        [Test]
        public async Task GetBeerByIdAsync_ReturnsOkResult_WithErrorMessage_WhenBeerNotFound()
        {
            // Arrange
            _mockBeerService.Setup(service => service.GetBeerByIdAsync(1)).ReturnsAsync((BeerResponse)null);

            // Act
            var result = await _controller.GetBeerByIdAsync(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ApiResponse<BeerResponse>;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(Messages.RecordNotFound("Beer"), response.Message);
        }

        [Test]
        public async Task AddBeerAsync_ReturnsOkResult_WithSavedBeer()
        {
            // Arrange
            var beerRequest = new BeerRequest { Name = "New Beer", PercentageAlcoholByVolume = 5.0m };
            var savedBeer = new BeerResponse { Id = 1, Name = "New Beer", PercentageAlcoholByVolume = 5.0m };
            _mockBeerService.Setup(service => service.AddBeerAsync(beerRequest)).ReturnsAsync(savedBeer);

            // Act
            var result = await _controller.AddBeerAsync(beerRequest);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ApiResponse<BeerResponse>;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(savedBeer, response.Data);
        }

        [Test]
        public async Task AddBeerAsync_ReturnsBadRequest_WithErrorMessage_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.AddBeerAsync(new BeerRequest());

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            var response = badRequestResult.Value as ApiResponse<IEnumerable<BeerResponse>>;
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.AreEqual("Validation failed", response.Message);
        }

        [Test]
        public async Task UpdateBeer_ReturnsOkResult_WithUpdatedBeer()
        {
            // Arrange
            var beerRequest = new BeerRequest { Id = 1, Name = "Updated Beer", PercentageAlcoholByVolume = 6.0m };
            var updatedBeer = new BeerResponse { Id = 1, Name = "Updated Beer", PercentageAlcoholByVolume = 6.0m };
            _mockBeerService.Setup(service => service.UpdateBeerAsync(beerRequest)).ReturnsAsync(updatedBeer);

            // Act
            var result = await _controller.UpdateBeer(1, beerRequest);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ApiResponse<BeerResponse>;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(updatedBeer, response.Data);
        }

        [Test]
        public async Task UpdateBeer_ReturnsBadRequest_WithErrorMessage_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.UpdateBeer(1, new BeerRequest());

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            var response = badRequestResult.Value as ApiResponse<IEnumerable<BeerResponse>>;
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.AreEqual("Validation failed", response.Message);
        }


    }
}
