using Apollon.Api.Mappers.Status;
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
            return Ok(_searchEngine.GetStatus(true).ToDto());
        }
    }
}