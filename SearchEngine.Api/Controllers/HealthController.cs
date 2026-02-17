using SearchEngine.Core.Search;
using Microsoft.AspNetCore.Mvc;

namespace SearchEngine.Api.Controllers {
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase {
        private Engine _searchEngine;

        public HealthController(Engine searchEngine) {
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