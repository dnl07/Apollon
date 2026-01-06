using Apollon.Core.Documents;
using Apollon.Models;
using Microsoft.AspNetCore.Mvc;

namespace Apollon.Controllers {

    [ApiController]
    [Route("documents")]
    public class DocumentsController : ControllerBase {
        public static readonly List<SearchDocument> Documents = [];

        [HttpPost]
        public IActionResult Add([FromBody] SearchDocument document) {
            Documents.Add(document);
            return Ok(new { status = "indexed", document.Id });
        }
    }
}
