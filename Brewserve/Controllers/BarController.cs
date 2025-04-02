using Brewserve.Core.Payloads;
using Brewserve.Core.Interfaces;
using Brewserve.Core.Services;
using Brewserve.Data.Models;
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

        /// <summary>
        /// Initializes a new instance of the "BeerController" class.
        /// </summary>
        /// <param name="barService">The bar service.</param>
        /// <param name="logger">The logger.</param>
        public BarController(IBarService barService, ILogger<BarController> logger)
        {
            _barService = barService;
            _logger = logger;
        }

        /// <summary>
        /// Get all bars
        /// </summary>
        /// <returns>A list of all bars.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<BarResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<BarResponse>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetBarsAsync()
        {
            _logger.LogInformation("Fetching all bars");
            var bars = await _barService.GetBarsAsync();
            if (bars == null || !bars.Any())
            {
                var errorResponse = new ApiResponse<BarResponse>("Bar not found");
                return Ok(errorResponse);
            }
            
            var response = new ApiResponse<IEnumerable<BarResponse>>(bars);
            return Ok(response);
        }

        /// <summary>
        /// Get bar by Id
        /// </summary>
        /// <param name="id">The ID of the bar to retrieve.</param>
        /// <returns>The bar details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<BarResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<BarResponse>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BarResponse>> GetBarByIdAsync(int id)
        {
            var bar = await _barService.GetBarByIdAsync(id);
            if (bar == null)
            {
                var errorResponse = new ApiResponse<BarResponse>("Bar not found");
                return Ok(errorResponse);
            }
            var response = new ApiResponse<BarResponse>(bar, "Request Successful");
            return Ok(response);
        }

        /// <summary>
        /// Insert a single bar
        /// </summary>
        /// <param name="bar">The bar details.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<BarResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<BarResponse>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BarResponse>> AddBarAsync(CreateBarRequest bar)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var errorResponse = new ApiResponse<IEnumerable<BarResponse>>(errors);

                return BadRequest(errorResponse);
            }
            var savedBar = await _barService.AddBarAsync(bar);
            if (savedBar == null)
            {
                var errorResponse = new ApiResponse<BarResponse>("Record already exists");
                return BadRequest(errorResponse);
            }
            var response = new ApiResponse<BarResponse>(savedBar);

            return Ok(response);
        }

        /// <summary>
        /// Update a bar by Id
        /// </summary>
        /// <param name="id">The ID of the bar to update.</param>
        /// <param name="bar">The updated bar details.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBar(int id, BarRequest bar)
        {
            if (!ModelState.IsValid && id != bar.Id)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                var errorResponse = new ApiResponse<IEnumerable<BarResponse>>(errors);

                return BadRequest(errorResponse);
            }
            await _barService.UpdateBarAsync(bar);
            var response = new ApiResponse<BarResponse>("${barId} updated successfully");

            return Ok(response);
        }
    }
}
