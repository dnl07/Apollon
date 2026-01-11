using Apollon.Core.Documents;

namespace Apollon.Api.Models {
    public class SearchResponse {
        public string Query { get; set; } = "";
        public string[] UsedTokes{ get; set; } = [];
        public List<SearchDocument> Docs { get; set; } = [];
    }
}