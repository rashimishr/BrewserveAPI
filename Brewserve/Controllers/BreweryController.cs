using Brewserve.Core.DTOs;
using Brewserve.Core.Interfaces;
using Brewserve.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Brewserve.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BreweryController : ControllerBase
    {
        private readonly IBreweryService _breweryService;

        public BreweryController(IBreweryService breweryService)
        {
            _breweryService = breweryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BreweryDTO>>> GetBreweriesAsync()
        {
            var breweries = await _breweryService.GetBreweriesAsync();
            return Ok(breweries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BreweryDTO>> GetBreweryByIdAsync(int id)
        {
            var brewery = await _breweryService.GetBreweryByIdAsync(id);
            if (brewery == null)
            {
                return NotFound();
            }
            return Ok(brewery);
        }

        [HttpPost]   
        public async Task<ActionResult<BreweryDTO>> AddBreweryAsync(CreateBreweryDTO breweryDto)
        {
            if (breweryDto == null)
            {
                return BadRequest();
            }
             
            await _breweryService.AddBreweryAsync(breweryDto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBrewery(int breweryId, BreweryDTO breweryDto)
        {
            if (breweryDto == null || breweryId != breweryDto.Id)
            {
                return BadRequest();
            }
            await _breweryService.UpdateBreweryAsync(breweryId, breweryDto);
            return NoContent();
        }
    }
}
