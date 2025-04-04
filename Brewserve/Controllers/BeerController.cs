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
    [Produces("application/json")]
    public class BeerController : ControllerBase
    {
        private readonly IBeerService _beerService;
        private readonly IBeerSearchStrategy _strategy;
        private readonly ILogger<BeerController> _logger;
        
        /// <summary>
        /// Initializes a new instance of the "BeerController"/> class.
        /// </summary>
        /// <param name="beerService">The beer service.</param>
        /// <param name="strategy">The strategy for filtering beers by alcohol content.</param>
        /// <param name="logger">The logger.</param>
        public BeerController(IBeerService beerService, IBeerSearchStrategy strategy, ILogger<BeerController> logger)
        {
            _beerService = beerService;
            _strategy = strategy;
            _logger = logger;
        }
        
        /// <summary>
        /// Get all beers with optional filtering query parameters for alcohol content
        ///                       (gtAlcoholByVolume = greater than, ltAlcoholByVolume = less than)
        /// </summary>
        /// <param name="gtAlcoholByVolume">The minimum alcohol content.</param>
        /// <param name="ltAlcoholByVolume">The maximum alcohol content.</param>
        /// <returns>A list of beers matching the specified alcohol content range.</returns>
        /// <response code="400">Bad Request</response>
        [HttpGet]
        [ProducesResponseType(typeof(BeerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<BeerResponse>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetBeers([FromQuery] decimal? gtAlcoholByVolume, [FromQuery] decimal? ltAlcoholByVolume)
        {
            _logger.LogInformation("Fetching beer with filter");
            //set the strategy
            _strategy.SetAlcoholContent(gtAlcoholByVolume, ltAlcoholByVolume);
            var beers = await _strategy.FilterAsync();
            if (!beers.Any() || beers == null)
            {
                var errorResponse = new ApiResponse<BeerResponse>( Messages.RecordNotFound("Beer"));
                return NotFound(errorResponse);
            }
            var response = new ApiResponse<IEnumerable<BeerResponse>>(beers);
            return Ok(response);
        }
        
        /// <summary>
        /// Get beer by id
        /// </summary>
        /// <param name="id">The ID of the beer to retrieve.</param>
        /// <returns>The beer details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<BeerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<BeerResponse>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BeerResponse>> GetBeerByIdAsync(int id)
        {
            _logger.LogInformation("Fetching beer with ID {BeerId}", id);
            var beer = await _beerService.GetBeerByIdAsync(id);
            if (beer == null)
            {
                var errorResponse = new ApiResponse<BeerResponse>(Messages.RecordNotFound("Beer"));
                _logger.LogError("Error occured while fetching beers for beer {BeerId}", id);
                return NotFound(errorResponse);
            }
            var response = new ApiResponse<BeerResponse>(beer);
            return Ok(response);
        }
        
        /// <summary>
        /// Insert a single beer
        /// </summary>
        /// <param name="beer">The beer details.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<BeerResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<BreweryResponse>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BeerResponse>> AddBeerAsync(BeerRequest beer)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var errorResponse = new ApiResponse<IEnumerable<BeerResponse>>(errors);
                _logger.LogError("Error occured while adding beer validation for beer {BeerId}", beer.Id);
                return BadRequest(errorResponse);
            }
            _logger.LogInformation("Adding a new beer");
            var savedBeer = await _beerService.AddBeerAsync(beer);
            if (savedBeer == null)
            {
                var errorResponse = new ApiResponse<BeerResponse>(new(),Messages.RecordAlreadyExists("Beer"));
                return Conflict(errorResponse);
            }
            var response = new ApiResponse<BeerResponse>(savedBeer);
            return Created("Beer", response);
        }
      
        /// <summary>
        /// Update a beer by id
        /// </summary>
        /// <param name="id">The ID of the beer to update.</param>
        /// <param name="beer">The updated beer details.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<BeerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<BeerResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBeer(int id, BeerRequest beer)
        {
            if (!ModelState.IsValid || id <= 0)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                if (id <= 0) errors.Add(Messages.IdGreaterThanZero("Beer"));
                var errorResponse = new ApiResponse<IEnumerable<BeerResponse>>(errors);
                _logger.LogError("Error occured while updating beer {BeerId}", id);
                return BadRequest(errorResponse);
            }
            _logger.LogInformation("Updating beer with ID {BeerId}", id);
            beer.Id = id;
            var updatedBeer = await _beerService.UpdateBeerAsync(beer);
            var response = new ApiResponse<BeerResponse>(updatedBeer, Messages.RecordUpdated("Beer", id));
            return Ok(response);
        }
    }
}
