using Apollon.Core.Search;
using Apollon.Models.Api;
using Apollon.Models.Search;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            var watch = new Stopwatch();
            SearchResult searchResult = _searchEngine.Search(request.Query, request.Options);
            watch.Stop();
            Debug.WriteLine($"Search took {watch.ElapsedMilliseconds}ms");
            return Ok(searchResult);
        }
    }
}
