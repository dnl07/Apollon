using Apollon.Api.Models;
using Apollon.Core.Documents;
using Apollon.Core.Search;
using Microsoft.AspNetCore.Mvc;

namespace Apollon.Api.Controllers {

    [ApiController]
    [Route("documents")]
    public class DocumentsController : ControllerBase {
        private readonly SearchEngine _searchEngine;

        public DocumentsController(SearchEngine searchEngine) {
            _searchEngine = searchEngine; 
        }

        [HttpPost]
        public IActionResult<SearchResponse> Add([FromBody] SearchDocument document) {
            _searchEngine.AddDocument(document);
            
            return Ok();
        }
    }
}
