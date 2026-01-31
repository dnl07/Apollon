using Apollon.Api.Dto.Search;
using Apollon.Api.Mappers.Options;
using Apollon.Api.Mappers.Search;
using Apollon.Core.Search;
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

        [HttpGet]
        public ActionResult<SearchResponseDto> Search([FromQuery] string query, bool explain = false) {
            var watch = new Stopwatch();
            watch.Start();
            SearchResult searchResult = _searchEngine.Search(query, explain, new QueryOptionsDto().ToEngineModel());
            watch.Stop();
            
            var searchResponse = searchResult.ToDto();
            searchResponse.ElapsedTime = watch.ElapsedMilliseconds;

            return Ok(searchResponse); 
        }

        [HttpPost]
        public ActionResult<SearchResponseDto> Search([FromBody] SearchRequestDto request) {
            var watch = new Stopwatch();
            watch.Start();
            SearchResult searchResult = _searchEngine.Search(request.Query, request.Options.Explain, request.Options.ToEngineModel());
            watch.Stop();
            
            var searchResponse = searchResult.ToDto();
            searchResponse.ElapsedTime = watch.ElapsedMilliseconds;

            return Ok(searchResponse); 
        }
    }
}
