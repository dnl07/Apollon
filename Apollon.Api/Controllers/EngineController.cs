using Apollon.Api.Dto.Engine;
using Apollon.Api.Mappers.Status;
using Apollon.Core.Options;
using Apollon.Core.Search;
using Microsoft.AspNetCore.Mvc;

namespace Apollon.Api.Controllers {
    [ApiController]
    [Route("engine")]
    public class EngineController : ControllerBase {
        private readonly SearchEngine _searchEngine;

        public EngineController(SearchEngine searchEngine) {
            _searchEngine = searchEngine; 
        }

        [HttpPost("init")]
        public IActionResult Add([FromBody] IndexOptions options) {
            _searchEngine.Initialize(options);
            return Ok(new { status = "options changed" });
        }

        [HttpGet("status")]
        public ActionResult<StatusDto> GetStatus() {
            return _searchEngine.GetStatus().ToDto();
        }
    }
}
