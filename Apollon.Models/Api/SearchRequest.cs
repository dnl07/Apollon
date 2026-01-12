using Apollon.Core.Options;

namespace Apollon.Models.Api {
    public class SearchRequest {
        public string Query { get; set; } = "";
        public QueryOptions Options { get; set; } = new();
    }
}
