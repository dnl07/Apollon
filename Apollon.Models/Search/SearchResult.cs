using Apollon.Core.Documents;

namespace Apollon.Models.Search {
    public class SearchResult {
        public string Query { get; set; } = "";
        public List<string> UsedTokens { get; set; } = [];
        public List<SearchDocument> Documents { get; set; } = new();
        }
}
