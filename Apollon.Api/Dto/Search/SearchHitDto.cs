namespace Apollon.Api.Dto.Search {
    public class SearchHitDto {
        public Guid Id = Guid.Empty;
        public SearchFieldDto Fields { get; set; } = new();
        public SearchExplainDto? Explain { get; set; } = null;
    }
}