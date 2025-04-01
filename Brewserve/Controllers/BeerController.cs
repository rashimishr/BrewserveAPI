using Brewserve.Core.DTOs;
using Brewserve.Core.Interfaces;
using Brewserve.Core.Strategies;
using Microsoft.AspNetCore.Mvc;

namespace Brewserve.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BeerController : ControllerBase
    {
        private readonly IBeerService _beerService;
        private readonly BeerByAlcoholContentStrategy _strategy;

        public BeerController(IBeerService beerService, BeerByAlcoholContentStrategy strategy)
        {
            _beerService = beerService;
            _strategy = strategy;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BeerDTO>>> GetBeersAsync()
        {
            var beers = await _beerService.GetBeersAsync();
            var response = new ApiResponse<IEnumerable<BeerDTO>>(beers);

            return Ok(beers);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BeerDTO>>> GetBeers([FromQuery] decimal gtAlcoholByVolume , [FromQuery] decimal ltAlcoholByVolume)
        {
            if (gtAlcoholByVolume <= 0 || ltAlcoholByVolume <= 0)
            {
                return BadRequest("Alcohol content must be greater than 0");
            }
            //set the strategy
            _strategy.SetAlcoholContent(gtAlcoholByVolume, ltAlcoholByVolume);
            var beers = await
                _strategy.FilterAsync();
            if (!beers.Any())
            {
                return NotFound($"No beer found with alcohol content >={gtAlcoholByVolume}<={ltAlcoholByVolume}");
            }
            return Ok(beers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BeerDTO>> GetBeerByIdAsync(int id)
        {
            var beer = await _beerService.GetBeerByIdAsync(id);
            if (beer == null)
            {
                var errorResponse = new ApiResponse<BeerDTO>(new List<string>{"Beer not found"});
                return NotFound();
            }
            return Ok(beer);
        }

        [HttpPost]
        public async Task<ActionResult<BeerDTO>> AddBeerAsync(CreateBeerDTO beerDto)
        {
            if (beerDto == null)
            {
                return BadRequest();
            }
            var beer = await _beerService.AddBeerAsync(beerDto);
            return Ok(beer);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateBeer(int beerId, BeerDTO beerDto)
        {
            if (beerDto == null || beerId != beerDto.Id)
            {
                return BadRequest();
            }
            await _beerService.UpdateBeerAsync(beerDto);
            return NoContent();
        }
    }
}
