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
        public ActionResult<SearchResponse> Search([FromBody] SearchRequest request) {
            var watch = new Stopwatch();
            watch.Start();
            SearchResult searchResult = _searchEngine.Search(request.Query, request.Options);
            watch.Stop();
            
            var searchResponse = new SearchResponse();
             
            searchResponse.Query = searchResult.Query;
            searchResponse.UsedTokes = searchResult.UsedTokens;
            searchResponse.Docs = searchResult.Documents;
            searchResponse.ElapsedTime = watch.ElapsedMilliseconds;

            return Ok(searchResponse);
        }
    }
}
