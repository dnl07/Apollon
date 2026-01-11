using Apollon.Core.Search;
using Apollon.Models.Api;
using Apollon.Models.Search;
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
        public ActionResult<SearchResult> Search([FromBody] SearchRequest request) {
            SearchResult searchResult = _searchEngine.Search(request.Query, request.MaxDocs);

            return Ok(
                new {
                    query = request.Query,
                    message = searchResult
                });
        }
    }
}
