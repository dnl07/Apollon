using Apollon.Api.Models;
using Apollon.Core.Documents;
using Apollon.Core.Indexing;
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
        public ActionResult<SearchResponse> Search([FromBody] SearchRequest request) {
            List<SearchDocument> documents = _searchEngine.Search(request.Query, request.MaxDocs);

            return Ok(
                new {
                    query = request.Query,
                    message = documents
                });
        }
    }
}
