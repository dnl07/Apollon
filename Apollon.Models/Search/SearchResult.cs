using Apollon.Core.Documents;

namespace Apollon.Models.Search {
    public class SearchResult {
        public string Query { get; set; } = "";
        public List<SearchHit> Hits { get; set; } = new();
    }
}
