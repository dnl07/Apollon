namespace Apollon.Api.Dto.Search {
    public class SearchResponseDto {
        public string Query { get; set; } = "";
        public int Total { get; set; } = 0;
        public long ElapsedTime { get; set; } = 0;
        public SearchHitDto[] Hits { get; set; } = [];
    }
}