using SearchEngine.Api.Dto.Engine;
using SearchEngine.Api.Mappers.Status;
using SearchEngine.Core.Options;
using SearchEngine.Core.Search;
using Microsoft.AspNetCore.Mvc;

namespace SearchEngine.Api.Controllers {
    [ApiController]
    [Route("engine")]
    public class EngineController : ControllerBase {
        private readonly Engine _searchEngine;

        public EngineController(Engine searchEngine) {
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
