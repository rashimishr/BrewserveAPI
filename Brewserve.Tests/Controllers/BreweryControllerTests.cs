using BrewServe.API.Controllers;
using BrewServe.Core.Constants;
using BrewServe.Core.Interfaces;
using BrewServe.Core.Payloads;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BrewServe.Tests.Controllers
{
    [TestFixture]
    public class BreweryControllerTests
    {
        private Mock<IBreweryService> _mockBreweryService;
        private Mock<ILogger<BreweryController>> _mockLogger;
        private BreweryController _controller;

        [SetUp]
        public void Setup()
        {
            _mockBreweryService = new Mock<IBreweryService>();
            _mockLogger = new Mock<ILogger<BreweryController>>();
            _controller = new BreweryController(_mockBreweryService.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetBreweriesAsync_ReturnsOkResult_WithBreweries()
        {
            // Arrange
            var breweries = new List<BreweryResponse>
            {
                new BreweryResponse { Id = 1, Name = "Brewery1" },
                new BreweryResponse { Id = 2, Name = "Brewery2" }
            };
            _mockBreweryService.Setup(service => service.GetBreweriesAsync()).ReturnsAsync(breweries);

            // Act
            var result = await _controller.GetBreweriesAsync();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ApiResponse<IEnumerable<BreweryResponse>>;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(breweries, response.Data);
        }

        [Test]
        public async Task GetBreweriesAsync_ReturnsNotFoundResult_WithEmptyList_WhenNoBreweriesFound()
        {
            // Arrange
            _mockBreweryService.Setup(service => service.GetBreweriesAsync()).ReturnsAsync(new List<BreweryResponse>());

            // Act
            var result = await _controller.GetBreweriesAsync();

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            var response = notFoundResult.Value as ApiResponse<IEnumerable<BreweryResponse>>;
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.IsNull(response.Data);
            Assert.AreEqual("Brewery not found", response.Errors[0]);
        }

        [Test]
        public async Task GetBreweryByIdAsync_ReturnsOkResult_WithBrewery()
        {
            // Arrange
            var brewery = new BreweryResponse { Id = 1, Name = "Brewery1" };
            _mockBreweryService.Setup(service => service.GetBreweryByIdAsync(1)).ReturnsAsync(brewery);

            // Act
            var result = await _controller.GetBreweryByIdAsync(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ApiResponse<BreweryResponse>;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(brewery, response.Data);
        }

        [Test]
        public async Task GetBreweryByIdAsync_ReturnsNotFoundResult_WithErrorMessage_WhenBreweryNotFound()
        {
            // Arrange
            _mockBreweryService.Setup(service => service.GetBreweryByIdAsync(1)).ReturnsAsync((BreweryResponse)null);

            // Act
            var result = await _controller.GetBreweryByIdAsync(1);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            var response = notFoundResult.Value as ApiResponse<BreweryResponse>;
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.AreEqual(Messages.RecordNotFound("Brewery"), response.Errors[0]);
        }

        [Test]
        public async Task AddBreweryAsync_ReturnsOkResult_WithSavedBrewery()
        {
            // Arrange
            var breweryRequest = new BreweryRequest { Name = "New Brewery" };
            var savedBrewery = new BreweryResponse { Id = 1, Name = "New Brewery" };
            _mockBreweryService.Setup(service => service.AddBreweryAsync(breweryRequest)).ReturnsAsync(savedBrewery);

            // Act
            var result = await _controller.AddBreweryAsync(breweryRequest);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ApiResponse<BreweryResponse>;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(savedBrewery, response.Data);
        }

        [Test]
        public async Task AddBreweryAsync_ReturnsBadRequest_WithErrorMessage_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.AddBreweryAsync(new BreweryRequest());

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            var response = badRequestResult.Value as ApiResponse<IEnumerable<BreweryResponse>>;
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.IsTrue(response.Message.Contains("Validation failed"));
        }

        [Test]
        public async Task UpdateBrewery_ReturnsOkResult_WithUpdatedBrewery()
        {
            // Arrange
            var breweryRequest = new BreweryRequest { Id = 1, Name = "Updated Brewery" };
            var updatedBrewery = new BreweryResponse { Id = 1, Name = "Updated Brewery" };
            _mockBreweryService.Setup(service => service.UpdateBreweryAsync(breweryRequest)).ReturnsAsync(updatedBrewery);

            // Act
            var result = await _controller.UpdateBrewery(1, breweryRequest);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ApiResponse<BreweryResponse>;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(updatedBrewery, response.Data);
        }

        [Test]
        public async Task UpdateBrewery_ReturnsBadRequest_WithErrorMessage_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.UpdateBrewery(1, new BreweryRequest());

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            var response = badRequestResult.Value as ApiResponse<IEnumerable<BreweryResponse>>;
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Test]
        public async Task AddBreweryBeerLinkAsync_ReturnsOkResult_WithLink()
        {
            // Arrange
            var linkRequest = new BreweryBeerLinkRequest { BreweryId = 1, BeerId = 1 };
            var linkResponse = new BreweryResponse { Id = 1, Name = "Brewery1" };
            _mockBreweryService.Setup(service => service.AddBreweryBeerLinkAsync(linkRequest)).ReturnsAsync(linkResponse);

            // Act
            var result = await _controller.AddBreweryBeerLinkAsync(linkRequest);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ApiResponse<BreweryResponse>;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(linkResponse, response.Data);
        }

        [Test]
        public async Task AddBreweryBeerLinkAsync_ReturnsBadRequest_WithErrorMessage_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("BreweryId", "Required");

            // Act
            var result = await _controller.AddBreweryBeerLinkAsync(new BreweryBeerLinkRequest());

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            var response = badRequestResult.Value as ApiResponse<IEnumerable<BreweryResponse>>;
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Test]
        public async Task GetAssociatedBreweryBeersAsync_ReturnsOkResult_WithLinks()
        {
            // Arrange
            var links = new List<BreweryBeerLinkResponse>
            {
                new BreweryBeerLinkResponse { Id = 1, Name = "Brewery1", Beers = new List<BeerResponse> { new BeerResponse { Id = 1, Name = "Beer1" } } },
                new BreweryBeerLinkResponse { Id = 2, Name = "Brewery2", Beers = new List<BeerResponse> { new BeerResponse { Id = 2, Name = "Beer2" } } }
            };
            _mockBreweryService.Setup(service => service.GetBreweryBeerLinkAsync()).ReturnsAsync(links);

            // Act
            var result = await _controller.GetAssociatedBreweryBeersAsync();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ApiResponse<IEnumerable<BreweryBeerLinkResponse>>;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(links, response.Data);
        }

        [Test]
        public async Task GetAssociatedBreweryBeersAsync_ReturnsNotFoundResult_WithErrorMessage_WhenNoLinksFound()
        {
            // Arrange
            _mockBreweryService.Setup(service => service.GetBreweryBeerLinkAsync()).ReturnsAsync(new List<BreweryBeerLinkResponse>());

            // Act
            var result = await _controller.GetAssociatedBreweryBeersAsync();

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            var response = notFoundResult.Value as ApiResponse<IEnumerable<BreweryBeerLinkResponse>>;
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.AreEqual(Messages.RecordNotFound("Brewery"), response.Errors[0]);
        }

        [Test]
        public async Task GetAssociatedBreweryBeerByBreweryIdAsync_ReturnsOkResult_WithLink()
        {
            // Arrange
            var link = new BreweryBeerLinkResponse { Id = 1, Name = "Brewery1", Beers = new List<BeerResponse> { new BeerResponse { Id = 1, Name = "Beer1" } } };
            _mockBreweryService.Setup(service => service.GetBreweryBeerLinkByBreweryIdAsync(1)).ReturnsAsync(link);

            // Act
            var result = await _controller.GetAssociatedBreweryBeerByBreweryIdAsync(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var response = okResult.Value as ApiResponse<BreweryBeerLinkResponse>;
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual(link, response.Data);
        }

        [Test]
        public async Task GetAssociatedBreweryBeerByBreweryIdAsync_ReturnsNotFountResult_WithErrorMessage_WhenLinkNotFound()
        {
            // Arrange
            _mockBreweryService.Setup(service => service.GetBreweryBeerLinkByBreweryIdAsync(1)).ReturnsAsync((BreweryBeerLinkResponse)null);

            // Act
            var result = await _controller.GetAssociatedBreweryBeerByBreweryIdAsync(1);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            var response = notFoundResult.Value as ApiResponse<BreweryBeerLinkResponse>;
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.AreEqual(Messages.RecordNotFound("Brewery"), response.Errors[0]);
        }

    }
}
