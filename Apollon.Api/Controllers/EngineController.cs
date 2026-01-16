using Apollon.Core.Options;
using Apollon.Core.Search;
using Microsoft.AspNetCore.Mvc;

namespace Apollon.Api.Controllers {
    [ApiController]
    [Route("engine")]
    public class OptionsController : ControllerBase {
        private readonly SearchEngine _searchEngine;

        public OptionsController(SearchEngine searchEngine) {
            _searchEngine = searchEngine; 
        }

        [HttpPost("init")]
        public IActionResult Add([FromBody] IndexOptions options) {
           



            _searchEngine.Initialize(options);
            return Ok(new { status = "options changed" });
        }
    }
}
