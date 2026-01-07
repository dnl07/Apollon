using Apollon.Api.Models;
using Apollon.Core.Search;
using Microsoft.AspNetCore.Mvc;

namespace Apollon.Api.Controllers {

    [ApiController]
    [Route("search")]
    public class SearchController : ControllerBase {
       private readonly SearchEngine _searchEngine;
        public SearchController(SearchEngine searchEngine) {
            _searchEngine = searchEngine;
        }

        [HttpPost]
        public IActionResult Search([FromBody] SearchRequest request) {
            SearchResponse response = _searchEngine.Search(request);

            return Ok(
                new {
                    query = request.Query,
                    message = "Apollon is listening"
                });
        }
    }
}
