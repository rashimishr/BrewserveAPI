using Brewserve.Core.Payloads;
using Brewserve.Core.Interfaces;
using Brewserve.Core.Strategies;
using Microsoft.AspNetCore.Mvc;
using Brewserve.Data.Models;

namespace Brewserve.API.Controllers
{
    /// <summary>
    /// Controller for managing beers.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BeerController : ControllerBase
    {
        private readonly IBeerService _beerService;
        private readonly BeerByAlcoholContentStrategy _strategy;
        private readonly ILogger<BarController> _logger;

        /// <summary>
        /// Initializes a new instance of the "BeerController"/> class.
        /// </summary>
        /// <param name="beerService">The beer service.</param>
        /// <param name="strategy">The strategy for filtering beers by alcohol content.</param>
        /// <param name="logger">The logger.</param>
        public BeerController(IBeerService beerService, BeerByAlcoholContentStrategy strategy, ILogger<BarController> logger)
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetBeers([FromQuery] decimal gtAlcoholByVolume , [FromQuery] decimal ltAlcoholByVolume)
        {
            if (gtAlcoholByVolume <= 0 || ltAlcoholByVolume <= 0)
            {
                return BadRequest(new  ApiResponse<List<BeerResponse>>("Alcohol content must be greater than 0"));
            }
            //set the strategy
            _strategy.SetAlcoholContent(gtAlcoholByVolume, ltAlcoholByVolume);
            var beers = await _strategy.FilterAsync();

            if (!beers.Any() || beers == null)
            {
                var errorResponse = new ApiResponse<BarResponse>($"No beer found with alcohol content >={ gtAlcoholByVolume } <= { ltAlcoholByVolume}");
                return Ok(errorResponse);
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
            var beer = await _beerService.GetBeerByIdAsync(id);
            if (beer == null)
            {
                var errorResponse = new ApiResponse<BeerResponse>("Beer not found");
                return Ok(errorResponse);
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
        [ProducesResponseType(typeof(ApiResponse<BeerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<BeerResponse>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BeerResponse>> AddBeerAsync(CreateBeerRequest beer)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var errorResponse = new ApiResponse<IEnumerable<BarResponse>>(errors);

                return BadRequest(errorResponse);
            }
            var savedBeer = await _beerService.AddBeerAsync(beer);
            if (savedBeer == null)
            {
                var errorResponse = new ApiResponse<BeerResponse>("Record already exists");
                return BadRequest(errorResponse);
            }
            var response = new ApiResponse<BeerResponse>(savedBeer);

            return Ok(response);
        }

        /// <summary>
        /// Update a beer by Id
        /// </summary>
        /// <param name="id">The ID of the beer to update.</param>
        /// <param name="beer">The updated beer details.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBeer(int id, BeerRequest beer)
        {
            if (!ModelState.IsValid && id != beer.Id)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                var errorResponse = new ApiResponse<IEnumerable<BarResponse>>(errors);

                return BadRequest(errorResponse);
            } 
            await _beerService.UpdateBeerAsync(beer);

            var response = new ApiResponse<BarResponse>("${beerId} updated successfully");

            return Ok(response);
        }
    }
}
