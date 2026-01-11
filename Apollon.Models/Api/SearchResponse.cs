using Apollon.Core.Documents;

namespace Apollon.Models.Api {
    public class SearchResponse {
        public string Query { get; set; } = "";
        public string[] UsedTokes{ get; set; } = [];
        public List<SearchDocument> Docs { get; set; } = [];
    }
}