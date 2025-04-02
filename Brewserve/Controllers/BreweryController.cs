using Brewserve.Core.Payloads;
using Brewserve.Core.Interfaces;
using Brewserve.Core.Services;
using Brewserve.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brewserve.API.Controllers
{
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
        [ProducesResponseType(typeof(ApiResponse<BreweryResponse>), StatusCodes.Status404NotFound)]
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
        /// Get brewery by Id
        /// </summary>
        /// <param name="id">The ID of the brewery to retrieve.</param>
        /// <returns>The brewery details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<BreweryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<BreweryResponse>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BreweryResponse>> GetBreweryByIdAsync(int id)
        {
            var brewery = await _breweryService.GetBreweryByIdAsync(id);
            if (brewery == null)
            {
                var errorResponse = new ApiResponse<BreweryResponse>(null,"Brewery not found");
                return Ok(errorResponse);
            }
            
            var response = new ApiResponse<BreweryResponse>(brewery, "Request Successful");
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
        public async Task<ActionResult<BreweryResponse>> AddBreweryAsync(CreateBreweryRequest brewery)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var errorResponse = new ApiResponse<IEnumerable<BreweryResponse>>(errors);

                return BadRequest(errorResponse);
            }
            var savedBrewery = await _breweryService.AddBreweryAsync(brewery);
            if (savedBrewery == null)
            {
                var errorResponse = new ApiResponse<BreweryResponse>("Record already exists");
                return BadRequest(errorResponse);
            }
            var response = new ApiResponse<BreweryResponse>(savedBrewery);

            return Ok(response);
        }

        /// <summary>
        /// Update a brewery by Id
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

                return BadRequest(errorResponse);
            }
            
            await _breweryService.UpdateBreweryAsync(brewery);
            var response = new ApiResponse<BreweryResponse>("${breweryId} updated successfully");

            return Ok(response);
        }


    }
}
