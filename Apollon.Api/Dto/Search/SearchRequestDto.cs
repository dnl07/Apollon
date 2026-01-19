using Apollon.Core.Options;

namespace Apollon.Api.Dto.Search {
    public class SearchRequestDto {
        public string Query { get; set; } = "";
        public QueryOptions Options { get; set; } = new();
    }
}
