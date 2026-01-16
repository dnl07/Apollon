using Apollon.Core.Documents;
using Apollon.Core.Search;
using Apollon.Models.Api;
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
        public ActionResult<DocumentResponse> Add([FromBody] SearchDocument document) {
            var doc = _searchEngine.AddDocument(document);

            var response = new DocumentResponse();
            response.Documents = [doc.Title];

            return Ok(new { status = "document added", response});
        }

        [HttpPost("bulk")]
        public ActionResult<DocumentResponse> AddBulk([FromBody] SearchDocument[] documents) {
            var response = new DocumentResponse();
            var watch = new Stopwatch();

            watch.Start();
            foreach (var doc in documents) {
                Guid id = _searchEngine.AddDocument(doc).Id;
                response.Documents.Add(id.ToString());
            }
            watch.Stop();

            response.ElapsedTime = watch.ElapsedMilliseconds;

            return Ok(new { status = $"{response.Documents.Count} documents added!", response });
        }
    }
}
