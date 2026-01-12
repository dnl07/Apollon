using Apollon.Core.Documents;
using Apollon.Core.Search;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Apollon.Api.Controllers {
    [ApiController]
    [Route("documents")]
    public class DocumentsController : ControllerBase {
        private readonly SearchEngine _searchEngine;

        public DocumentsController(SearchEngine searchEngine) {
            _searchEngine = searchEngine; 
        }

        [HttpPost]
        public IActionResult Add([FromBody] SearchDocument document) {
            var doc = _searchEngine.AddDocument(document);
            
            return Ok(new { status = "document added", id = doc.Id});
        }

        [HttpPost("bulk")]
        public IActionResult AddBulk([FromBody] SearchDocument[] documents) {
            var watch = new Stopwatch();
            watch.Start();
            Dictionary<Guid, string> ids = new Dictionary<Guid, string>();

            foreach (var doc in documents) {
                Guid id = _searchEngine.AddDocument(doc).Id;
                ids[id] = doc.Title;
            }

            watch.Stop();
            Debug.WriteLine($"Added Documents in {watch.ElapsedMilliseconds}ms");

            return Ok(new { status = "SUCCESS", ids });
        }
    }
}
