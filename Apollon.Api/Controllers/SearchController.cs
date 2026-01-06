using Apollon.Models;
using Microsoft.AspNetCore.Mvc;

namespace Apollon.Controllers {

    [ApiController]
    [Route("search")]
    public class SearchController : ControllerBase {
        [HttpPost]
        public IActionResult Search([FromBody] SearchRequest request) {
            return Ok(
                new {
                    query = request.Query,
                    message = "Apollon is listening"
                });
        }
    }
}
