using Apollon.Core.Search;
using Microsoft.AspNetCore.Mvc;

namespace Apollon.Api.Controllers {
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase {
        private SearchEngine _searchEngine;

        public HealthController(SearchEngine searchEngine) {
            _searchEngine = searchEngine; 
        }

        [HttpGet]
        public IActionResult GetHealth() {
            if (_searchEngine.GetStatus().IsRunning) {
                return Ok();
            }
            return NotFound();
        }
    }
}