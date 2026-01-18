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

        public IActionResult GetHealth() {
            return Ok();
        }
    }
}