using System.Diagnostics;
using Apollon.Api.Dto.Documents;
using Apollon.Api.Mappers.Document;
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

        [HttpPost("add")]
        public ActionResult<DocumentResponseDto> Add([FromBody] DocumentRequestDto document) {
            var watch = new Stopwatch();
            watch.Start();
            var doc =_searchEngine.AddDocument(document.ToEngineModel());
            watch.Stop();

            var response = new DocumentResponseDto {
                Status = "Successfully added",
                TotalAdded = 1,
                TookMs = watch.ElapsedMilliseconds,
                AddedDocuments = [doc.ToDto()]
            };

            return Ok(response);
        }

        [HttpPost("bulk")]
        public ActionResult<DocumentResponseDto> AddBulk([FromBody] DocumentRequestDto[] documents) {
            var dtos = new List<DocumentDto>();

            var watch = new Stopwatch();
            watch.Start();
            foreach (var doc in documents) {
                var addedDoc = _searchEngine.AddDocument(doc.ToEngineModel());
                dtos.Add(addedDoc.ToDto());
            }
            watch.Stop();

            var response = new DocumentResponseDto {
                Status = "Successfully added",
                TotalAdded = dtos.Count,
                TookMs = watch.ElapsedMilliseconds,
                AddedDocuments = dtos.ToArray()
            };

            return Ok(response);
        }

        [HttpPut("update/{id:Guid}")]
        public IActionResult Update(Guid id, [FromBody] DocumentRequestDto doc) {
            _searchEngine.UpdateDocument(id, doc.ToEngineModel());
            return Ok();
        }

 
        [HttpPut("remove/{id:Guid}")]
        public IActionResult Remove(Guid id) {
            _searchEngine.RemoveDocument(id);
            return Ok();
        }       
    }
}
