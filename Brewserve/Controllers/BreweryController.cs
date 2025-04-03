using BrewServe.Core.Constants;
using BrewServe.Core.Interfaces;
using BrewServe.Core.Payloads;
using Microsoft.AspNetCore.Mvc;

namespace BrewServe.API.Controllers
{
    /// <summary>
    /// Controller for managing beers.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BreweryController : ControllerBase
    {
        private readonly IBreweryService _breweryService;
        private readonly ILogger<BarController> _logger;

        /// <summary>
        /// Initializes a new instance of the "BreweryController"/> class.
        /// </summary>
        /// <param name="breweryService">The brewery service.</param>
        /// <param name="logger">The logger.</param>
        public BreweryController(IBreweryService breweryService, ILogger<BarController> logger)
        {
            _breweryService = breweryService;
            _logger = logger;
        }

        /// <summary>
        /// Get all breweries
        /// </summary>
        /// <returns>A list of all breweries.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<BreweryResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BreweryResponse>>> GetBreweriesAsync()
        {
            _logger.LogInformation("Fetching all breweries");
            var breweries = await _breweryService.GetBreweriesAsync();

            if (breweries == null || !breweries.Any())
            {
                var errorResponse = new ApiResponse<IEnumerable<BreweryResponse>>(Enumerable.Empty<BreweryResponse>(), "Brewery not found");
                return Ok(errorResponse);
            }
            var response = new ApiResponse<IEnumerable<BreweryResponse>>(breweries);

            return Ok(response);
        }

        /// <summary>
        /// Get brewery by id
        /// </summary>
        /// <param name="id">The ID of the brewery to retrieve.</param>
        /// <returns>The brewery details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<BreweryResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BreweryResponse>> GetBreweryByIdAsync(int id)
        {
            _logger.LogInformation("Fetching brewery with ID {BreweryId}", id);
            var brewery = await _breweryService.GetBreweryByIdAsync(id);
            if (brewery == null)
            {
                var errorResponse = new ApiResponse<BreweryResponse>(null, Messages.RecordNotFound("Brewery"));
                _logger.LogError("Brewery with ID {BreweryId} not found", id);
                return Ok(errorResponse);
            }
            var response = new ApiResponse<BreweryResponse>(brewery);
            return Ok(response);
        }

        /// <summary>
        /// Insert a single brewery
        /// </summary>
        /// <param name="brewery">The brewery details.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<BreweryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<BreweryResponse>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BreweryResponse>> AddBreweryAsync(BreweryRequest brewery)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var errorResponse = new ApiResponse<IEnumerable<BreweryResponse>>(errors);
                _logger.LogError("Invalid brewery data: {Errors}", errors);
                return BadRequest(errorResponse);
            }
            var savedBrewery = await _breweryService.AddBreweryAsync(brewery);
            if (savedBrewery == null)
            {
                var errorResponse = new ApiResponse<BreweryResponse>(Messages.RecordAlreadyExists("Brewery"));
                return BadRequest(errorResponse);
            }
            _logger.LogInformation("Brewery added successfully");
            var response = new ApiResponse<BreweryResponse>(savedBrewery);
            return Ok(response);
        }

        /// <summary>
        /// Update a brewery by id
        /// </summary>
        /// <param name="id">The ID of the brewery to update.</param>
        /// <param name="brewery">The updated brewery details.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBrewery(int id, BreweryRequest brewery)
        {
            if (!ModelState.IsValid && id != brewery.Id)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                var errorResponse = new ApiResponse<IEnumerable<BreweryResponse>>(errors);
                _logger.LogError("Invalid brewery data: {Errors}", errors);
                return BadRequest(errorResponse);
            }
            _logger.LogInformation("Updating brewery with ID {BreweryId}", id);
            brewery.Id = id;
            var updatedBrewery = await _breweryService.UpdateBreweryAsync(brewery);
            var response = new ApiResponse<BreweryResponse>(updatedBrewery, Messages.RecordUpdated("Brewery",id));
            return Ok(response);
        }

        /// <summary>
        /// Insert a single brewery beer link
        /// </summary>
        /// <param name="linkRequest">The brewery  beer link.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost("beer")]
        [ProducesResponseType(typeof(ApiResponse<BreweryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BreweryResponse>> AddBreweryBeerLinkAsync(BreweryBeerLinkRequest linkRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var errorResponse = new ApiResponse<IEnumerable<BreweryResponse>>(errors);
               _logger.LogError("Invalid brewery beer link data: {Errors}", errors);
                return BadRequest(errorResponse);
            }
            _logger.LogInformation("Adding a new brewery beer link");
            var link = await _breweryService.AddBreweryBeerLinkAsync(linkRequest);
            var response = new ApiResponse<BreweryResponse>(link);
            return Ok(response);
        }

        /// <summary>
        /// Get all breweries with associated beers
        /// </summary>
        /// <returns>A list of all breweries.</returns>
        [HttpGet("beer")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<BreweryBeerLinkResponse>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BreweryBeerLinkResponse>>> GetAssociatedBreweryBeersAsync()
        {
            _logger.LogInformation("Fetching all breweries and associated beers");
            var link = await _breweryService.GetBreweryBeerLinkAsync();

            if (link == null || !link.Any())
            {
                var errorResponse = new ApiResponse<IEnumerable<BreweryBeerLinkResponse>>(Messages.RecordNotFound("Brewery"));
                _logger.LogError("No breweries found with associated beers");
                return Ok(errorResponse);
            }
            var response = new ApiResponse<IEnumerable<BreweryBeerLinkResponse>>(link);
            return Ok(response);
        }

        /// <summary>
        /// Get a single brewery by id with associated beers
        /// </summary>
        /// <param name="breweryId">The ID of the brewery to retrieve.</param>
        /// <returns>The brewery beer link details.</returns>
        [HttpGet("{breweryId}/beer")]
        [ProducesResponseType(typeof(ApiResponse<BreweryBeerLinkResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<BreweryBeerLinkResponse>> GetAssociatedBreweryBeerByBreweryIdAsync(int breweryId)
        {
            _logger.LogInformation("Fetching brewery beer link with ID {BreweryId}", breweryId);
            var link = await _breweryService.GetBreweryBeerLinkByBreweryIdAsync(breweryId);
            if (link == null)
            {
                var errorResponse = new ApiResponse<BreweryBeerLinkResponse>(Messages.RecordNotFound("Brewery"));
                _logger.LogError("Brewery beer link with ID {BreweryId} not found", breweryId);
                return Ok(errorResponse);
            }
            var response = new ApiResponse<BreweryBeerLinkResponse>(link);
            return Ok(response);
        }
    }
}
