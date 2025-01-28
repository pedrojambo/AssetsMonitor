using Microsoft.AspNetCore.Mvc;
using AssetsMonitor.Data;
using System.Linq;
using System.Threading.Tasks;
using AssetsMonitor.Services;
using AssetsMonitor.Interfaces;

namespace AssetsMonitor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetsService _assetsService;
        private readonly ILogger<AssetsController> _logger;

        public AssetsController(IAssetsService assetsService, ILogger<AssetsController> logger)
        {
            _assetsService = assetsService;
            _logger = logger;
        }

        [HttpGet("getHistory")]
        public async Task<IActionResult> GetHistory([FromQuery] string symbol)
        {
            _logger.LogInformation("BEGIN: GetHistory for symbol {Symbol}", symbol);

            var globalQuotes = await _assetsService.GetHistoryAsync(symbol);

            if (globalQuotes == null || globalQuotes.Count == 0)
            {
                _logger.LogInformation("END: GetHistory for symbol {Symbol} - Not Found", symbol);
                return NotFound();
            }

            _logger.LogInformation("END: GetHistory for symbol {Symbol} - Success", symbol);
            return Ok(globalQuotes);
        }
    }
}
