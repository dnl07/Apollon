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
            var doc =_searchEngine.AddDocument(document.ToEngineModel());

            var response = new DocumentResponseDto {
                Status = "Successfully added",
                TotalAdded = 1,
                AddedDocuments = [doc.ToDto()]
            };

            return Ok(response);
        }

        [HttpPost("bulk")]
        public ActionResult<DocumentResponseDto> AddBulk([FromBody] DocumentRequestDto[] documents) {
            var dtos = new List<DocumentDto>();

            foreach (var doc in documents) {
                var addedDoc = _searchEngine.AddDocument(doc.ToEngineModel());
                dtos.Add(addedDoc.ToDto());
            }

            var response = new DocumentResponseDto {
                Status = "Successfully added",
                TotalAdded = dtos.Count,
                AddedDocuments = dtos.ToArray()
            };

            return Ok(response);
        }
    }
}
