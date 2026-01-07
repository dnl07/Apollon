using Apollon.Core.Documents;

namespace Apollon.Api.Models {
    public class SearchResponse {
        public List<SearchDocument> Docs { get; set; } = [];
    }
}