using Brewserve.Core.DTOs;
using Brewserve.Core.Interfaces;
using Brewserve.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brewserve.API.Controllers
{
    /// <summary>
    /// Controller for managing bars.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BarController:ControllerBase
    {
        private readonly IBarService _barService;
        private readonly ILogger<BarController> _logger;

        public BarController(IBarService barService, ILogger<BarController> logger)
        {
            _barService = barService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all bars.
        /// </summary>
        /// <returns>A list of all bars.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BarDTO>>> GetBarsAsync()
        {
            _logger.LogInformation("Fetching all bars");
            var bars = await _barService.GetBarsAsync();
            return Ok(bars);
        }

        /// <summary>
        /// Retrieves a bar by its ID.
        /// </summary>
        /// <param name="id">The ID of the bar to retrieve.</param>
        /// <returns>The bar details.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BarDTO>> GetBarByIdAsync(int id)
        {
            var bar = await _barService.GetBarByIdAsync(id);
            if (bar == null)
            {
                return NotFound();
            }   
            return Ok(bar);
        }

        /// <summary>
        /// Adds a new bar.
        /// </summary>
        /// <param name="barDto">The bar details.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<ActionResult<BarDTO>> AddBarAsync(CreateBarDTO barDto)
        {
            if(barDto == null)
            {
                return BadRequest();
            }   
            await _barService.AddBarAsync(barDto);
            return Ok();
        }

        /// <summary>
        /// Updates an existing bar.
        /// </summary>
        /// <param name="barId">The ID of the bar to update.</param>
        /// <param name="barDto">The updated bar details.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPut("{barId}")]
        public async Task<IActionResult> UpdateBar(int barId, BarDTO barDto)
        {
            if (barDto == null || barId != barDto.Id)
            {
                return BadRequest();
            }
             
            await _barService.UpdateBarAsync(barDto);
          
            return NoContent();
        }
    }
}
