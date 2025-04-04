using BrewServe.Core.Constants;
using BrewServe.Core.Interfaces;
using BrewServe.Core.Payloads;
using Microsoft.AspNetCore.Mvc;
namespace BrewServe.API.Controllers;

/// <summary>
///     Controller for managing bars.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BarController : ControllerBase
{
    private readonly IBarService _barService;
    private readonly ILogger<BarController> _logger;

    /// <summary>
    ///     Initializes a new instance of the "BarController" class.
    /// </summary>
    /// <param name="barService">The bar service.</param>
    /// <param name="logger">The logger.</param>
    public BarController(IBarService barService, ILogger<BarController> logger)
    {
        _barService = barService;
        _logger = logger;
    }

    /// <summary>
    ///     Get all bars
    /// </summary>
    /// <returns>A list of all bars.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<BarResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<BarResponse>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetBarsAsync()
    {
        _logger.LogInformation("Fetching all bars");
        var bars = await _barService.GetBarsAsync();
        if (bars == null || !bars.Any())
        {
            var errorResponse = new ApiResponse<BarResponse>(Messages.RecordNotFound("Bar"));
            return NotFound(errorResponse);
        }
        var response = new ApiResponse<IEnumerable<BarResponse>>(bars);
        return Ok(response);
    }

    /// <summary>
    ///     Get bar by id
    /// </summary>
    /// <param name="id">The ID of the bar to retrieve.</param>
    /// <returns>The bar details.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BarResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<BarResponse>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BarResponse>> GetBarByIdAsync(int id)
    {
        _logger.LogInformation("Fetching bar with ID {BarId}", id);
        var bar = await _barService.GetBarByIdAsync(id);
        if (bar == null)
        {
            var errorResponse = new ApiResponse<BarResponse>(Messages.RecordNotFound("Bar"));
            return NotFound(errorResponse);
        }
        var response = new ApiResponse<BarResponse>(bar);
        return Ok(response);
    }

    /// <summary>
    ///     Insert a single bar
    /// </summary>
    /// <param name="bar">The bar details.</param>
    /// <returns>A response indicating the result of the operation.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BarResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<BreweryResponse>), StatusCodes.Status409Conflict)]
    [ProducesResponseType( StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BarResponse>> AddBarAsync(BarRequest bar)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            var errorResponse = new ApiResponse<IEnumerable<BarResponse>>(errors);
            _logger.LogError("Error occured while adding bar validation");
            return BadRequest(errorResponse);
        }
        _logger.LogInformation("Adding a new bar");
        var savedBar = await _barService.AddBarAsync(bar);
        if (savedBar == null)
        {
            var errorResponse = new ApiResponse<BarResponse>(new(),Messages.RecordAlreadyExists("Bar"));
            return Conflict(errorResponse);
        }
        var response = new ApiResponse<BarResponse>(savedBar);
        return Created("Bar", response); ;
    }

    /// <summary>
    ///     Update a bar by id
    /// </summary>
    /// <param name="id">The ID of the bar to update.</param>
    /// <param name="bar">The updated bar details.</param>
    /// <returns>A response indicating the result of the operation. </returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<BarResponse>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<List<ApiResponse<BarResponse>>>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateBar(int id, BarRequest bar)
    {
        if (!ModelState.IsValid || id <= 0)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
            if (id <= 0) errors.Add(Messages.IdGreaterThanZero("Bar"));
            var errorResponse = new ApiResponse<IEnumerable<BarResponse>>(errors);
            _logger.LogError("Error occured while updating bar validation");
            return BadRequest(errorResponse);
        }
        _logger.LogInformation("Updating bar with ID {BarId}", id);
        bar.Id = id;
        await _barService.UpdateBarAsync(bar);
        var response = new ApiResponse<BarResponse>(Messages.RecordUpdated("Bar", id));
        return Ok(response);
    }

    /// <summary>
    ///     Insert a single bar beer link
    /// </summary>
    /// <param name="linkRequest">The bar  beer link.</param>
    /// <returns>A response indicating the result of the operation.</returns>
    [HttpPost("beer")]
    [ProducesResponseType(typeof(ApiResponse<BarResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<BarResponse>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddBarBeerLinkAsync(BarBeerLinkRequest linkRequest)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            var errorResponse = new ApiResponse<IEnumerable<BarResponse>>(errors);
            _logger.LogError("Error occured while adding beer-bar link validation");
            return BadRequest(errorResponse);
        }
        _logger.LogInformation("Adding a new bar beer link");
        var link = await _barService.AddBarBeerLinkAsync(linkRequest);
        var response = new ApiResponse<BarResponse>(link);
        return Ok(response);
    }

    /// <summary>
    ///     Get all bars with associated beers
    /// </summary>
    /// <returns>A list of all bars.</returns>
    [HttpGet("beer")]
    [ProducesResponseType(typeof(ApiResponse<BarBeerLinkResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<BarBeerLinkResponse>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<BarBeerLinkResponse>>> GetAssociatedBarBeersAsync()
    {
        _logger.LogInformation("Fetching all bars and associated beers");
        var link = await _barService.GetBarBeerLinkAsync();
        if (link == null || !link.Any())
        {
            var errorResponse = new ApiResponse<IEnumerable<BarBeerLinkResponse>>( Messages.RecordNotFound("Bar"));
            _logger.LogError("Error occured while fetching all beer-bar link validation");
            return NotFound(errorResponse);
        }
        var response = new ApiResponse<IEnumerable<BarBeerLinkResponse>>(link);
        return Ok(response);
    }

    /// <summary>
    ///     Get a single bar by id with associated beers
    /// </summary>
    /// <param name="barId">The ID of the bars to retrieve.</param>
    /// <returns>The bar beer link details.</returns>
    [HttpGet("{barId}/beer")]
    [ProducesResponseType(typeof(ApiResponse<BarBeerLinkResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<BarBeerLinkResponse>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BarBeerLinkResponse>> GetAssociatedBarBeerByBarIdAsync(int barId)
    {
        var link = await _barService.GetBarBeerLinkByBarIdAsync(barId);
        if (link == null)
        {
            var errorResponse = new ApiResponse<IEnumerable<BarBeerLinkResponse>>(Messages.RecordNotFound("Bar"));
            _logger.LogError("Error occured while fetching beer-bar link validation for bar {BarId}", barId);
            return NotFound(errorResponse);
        }
        _logger.LogInformation("Fetching bar beer link for bar ID {BarId}", barId);
        var response = new ApiResponse<BarBeerLinkResponse>(link);
        return Ok(response);
    }
}