using SearchEngine.Api.Dto.Search;
using SearchEngine.Api.Mappers.Options;
using SearchEngine.Api.Mappers.Search;
using SearchEngine.Core.Search;
using SearchEngine.Models.Search;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace SearchEngine.Api.Controllers {
    [ApiController]
    [Route("search")]
    public class SearchController : ControllerBase {
       private readonly Engine _searchEngine;
        public SearchController(Engine searchEngine) {
            _searchEngine = searchEngine;
        }

        [HttpGet]
        public ActionResult<SearchResponseDto> Search([FromQuery] string query, [FromQuery] int? limit, [FromQuery] bool explain = false) {
            var watch = new Stopwatch();
            watch.Start();
            SearchResult searchResult = _searchEngine.Search(query, explain, new QueryOptionsDto() { Limit = limit ?? 10 }.ToEngineModel());
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
