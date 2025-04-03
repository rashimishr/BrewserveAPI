using BrewServe.API.Controllers;
using BrewServe.Core.Constants;
using BrewServe.Core.Interfaces;
using BrewServe.Core.Payloads;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BrewServe.Tests.Controllers;

[TestFixture]
public class BarControllerTests
{
    [SetUp]
    public void Setup()
    {
        _mockBarService = new Mock<IBarService>();
        _mockLogger = new Mock<ILogger<BarController>>();
        _controller = new BarController(_mockBarService.Object, _mockLogger.Object);
    }

    private Mock<IBarService> _mockBarService;
    private Mock<ILogger<BarController>> _mockLogger;
    private BarController _controller;

    [Test]
    public async Task GetBarsAsync_ReturnsOkResult_WithBars()
    {
        // Arrange
        var bars = new List<BarResponse>
        {
            new() { Id = 1, Name = "Bar1" },
            new() { Id = 2, Name = "Bar2" }
        };
        _mockBarService.Setup(service => service.GetBarsAsync()).ReturnsAsync(bars);

        // Act
        var result = await _controller.GetBarsAsync();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<IEnumerable<BarResponse>>;
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.AreEqual(bars, response.Data);
    }

    [Test]
    public async Task GetBarsAsync_ReturnsOkResult_WithEmptyList_WhenNoBarsFound()
    {
        // Arrange
        _mockBarService.Setup(service => service.GetBarsAsync()).ReturnsAsync(new List<BarResponse>());

        // Act
        var result = await _controller.GetBarsAsync();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<BarResponse>;
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.IsNull(response.Data);
        Assert.AreEqual("Bar not found", response.Errors[0]);
    }

    [Test]
    public async Task GetBarByIdAsync_ReturnsOkResult_WithBar()
    {
        // Arrange
        var bar = new BarResponse { Id = 1, Name = "Bar1" };
        _mockBarService.Setup(service => service.GetBarByIdAsync(1)).ReturnsAsync(bar);

        // Act
        var result = await _controller.GetBarByIdAsync(1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<BarResponse>;
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.AreEqual(bar, response.Data);
    }

    [Test]
    public async Task GetBarByIdAsync_ReturnsOkResult_WithErrorMessage_WhenBarNotFound()
    {
        // Arrange
        _mockBarService.Setup(service => service.GetBarByIdAsync(1)).ReturnsAsync((BarResponse)null);

        // Act
        var result = await _controller.GetBarByIdAsync(1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<BarResponse>;
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.IsNull(response.Data);
        Assert.AreEqual(Messages.RecordNotFound("Bar"), response.Errors[0]);
    }

    [Test]
    public async Task AddBarAsync_ReturnsOkResult_WithSavedBar()
    {
        // Arrange
        var barRequest = new BarRequest { Name = "New Bar" };
        var savedBar = new BarResponse { Id = 1, Name = "New Bar" };
        _mockBarService.Setup(service => service.AddBarAsync(barRequest)).ReturnsAsync(savedBar);

        // Act
        var result = await _controller.AddBarAsync(barRequest);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<BarResponse>;
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.AreEqual(savedBar, response.Data);
    }

    [Test]
    public async Task AddBarAsync_ReturnsBadRequest_WithErrorMessage_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await _controller.AddBarAsync(new BarRequest());

        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        var response = badRequestResult.Value as ApiResponse<IEnumerable<string>>;
        Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Test]
    public async Task UpdateBar_ReturnsOkResult_WithUpdatedBar()
    {
        // Arrange
        var barRequest = new BarRequest { Id = 1, Name = "Updated Bar" };
        var updatedBar = new BarResponse { Id = 1, Name = "Updated Bar" };
        _mockBarService.Setup(service => service.UpdateBarAsync(barRequest)).ReturnsAsync(updatedBar);

        // Act
        var result = await _controller.UpdateBar(1, barRequest);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<BarResponse>;
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
    }

    [Test]
    public async Task UpdateBar_ReturnsBadRequest_WithErrorMessage_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await _controller.UpdateBar(1, new BarRequest());

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        var response = badRequestResult.Value as ApiResponse<IEnumerable<BarResponse>>;
        Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        Assert.IsNull(response.Data);
        Assert.AreEqual("Validation failed", response.Message);
    }

    [Test]
    public async Task AddBarBeerLinkAsync_ReturnsOkResult_WithLink()
    {
        // Arrange
        var linkRequest = new BarBeerLinkRequest { BarId = 1, BeerId = 1 };
        var linkResponse = new BarResponse { Id = 1, Name = "Bar1" };
        _mockBarService.Setup(service => service.AddBarBeerLinkAsync(linkRequest)).ReturnsAsync(linkResponse);

        // Act
        var result = await _controller.AddBarBeerLinkAsync(linkRequest);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<BarResponse>;
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.AreEqual(linkResponse, response.Data);
    }

    [Test]
    public async Task AddBarBeerLinkAsync_ReturnsBadRequest_WithErrorMessage_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("BarId", "Required");

        // Act
        var result = await _controller.AddBarBeerLinkAsync(new BarBeerLinkRequest());

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        var response = badRequestResult.Value as ApiResponse<IEnumerable<string>>;
        Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Test]
    public async Task GetAssociatedBarBeersAsync_ReturnsOkResult_WithLinks()
    {
        // Arrange
        var links = new List<BarBeerLinkResponse>
        {
            new() { Id = 1, Name = "Bar1", Beers = new List<BeerResponse> { new() { Id = 1, Name = "Beer1" } } },
            new() { Id = 2, Name = "Bar2", Beers = new List<BeerResponse> { new() { Id = 2, Name = "Beer2" } } }
        };
        _mockBarService.Setup(service => service.GetBarBeerLinkAsync()).ReturnsAsync(links);

        // Act
        var result = await _controller.GetAssociatedBarBeersAsync();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<IEnumerable<BarBeerLinkResponse>>;
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.AreEqual(links, response.Data);
    }

    [Test]
    public async Task GetAssociatedBarBeersAsync_ReturnsOkResult_WithErrorMessage_WhenNoLinksFound()
    {
        // Arrange
        _mockBarService.Setup(service => service.GetBarBeerLinkAsync()).ReturnsAsync(new List<BarBeerLinkResponse>());

        // Act
        var result = await _controller.GetAssociatedBarBeersAsync();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<IEnumerable<BarBeerLinkResponse>>;
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.IsEmpty(response.Data);
        Assert.AreEqual(Messages.RecordNotFound("Bar"), response.Message);
    }

    [Test]
    public async Task GetAssociatedBarBeerByBarIdAsync_ReturnsOkResult_WithLink()
    {
        // Arrange
        var link = new BarBeerLinkResponse
            { Id = 1, Name = "Bar1", Beers = new List<BeerResponse> { new() { Id = 1, Name = "Beer1" } } };
        _mockBarService.Setup(service => service.GetBarBeerLinkByBarIdAsync(1)).ReturnsAsync(link);

        // Act
        var result = await _controller.GetAssociatedBarBeerByBarIdAsync(1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<BarBeerLinkResponse>;
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.AreEqual(link, response.Data);
    }

    [Test]
    public async Task GetAssociatedBarBeerByBarIdAsync_ReturnsOkResult_WithErrorMessage_WhenLinkNotFound()
    {
        // Arrange
        _mockBarService.Setup(service => service.GetBarBeerLinkByBarIdAsync(1)).ReturnsAsync((BarBeerLinkResponse)null);

        // Act
        var result = await _controller.GetAssociatedBarBeerByBarIdAsync(1);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<IEnumerable<BarBeerLinkResponse>>;
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.IsNull(response.Data);
        Assert.AreEqual(Messages.RecordNotFound("Bar"), response.Message);
    }
}