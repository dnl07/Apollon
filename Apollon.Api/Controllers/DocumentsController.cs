using Apollon.Api.Dto.Documents;
using Apollon.Api.Mappers.Document;
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
        public ActionResult<DocumentResponseDto> Add([FromBody] DocumentRequestDto document) {
            var doc = _searchEngine.AddDocument(document.ToEngineModel());

            var response = new DocumentResponseDto {
                Status = "Successfully added",
                TotalAdded = 1,
                AddedDocuments = [doc.ToDto()]
            };

            return Ok(response);
        }

        [HttpPost("bulk")]
        public ActionResult<DocumentResponseDto> AddBulk([FromBody] SearchDocument[] documents) {
            var documentsDto = new List<DocumentDto>();
            
            var watch = new Stopwatch();
            watch.Start();
            foreach (var doc in documents) {
                documentsDto.Add(doc.ToDto());
            }
            watch.Stop();

            var response = new DocumentResponseDto {
                Status = "Successfully added",
                TotalAdded = documentsDto.Count,
                AddedDocuments = documentsDto.ToArray()
            };

            return Ok(response);
        }
    }
}
