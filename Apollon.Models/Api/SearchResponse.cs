using Apollon.Core.Documents;

namespace Apollon.Models.Api {
    public class SearchResponse {
        public string Query { get; set; } = "";
        public List<string> UsedTokes{ get; set; } = [];
        public List<SearchDocument> Docs { get; set; } = [];
        public float ElapsedTime { get; set; } = 0.0f;
    }
}