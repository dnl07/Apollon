namespace Apollon.Models {
    public class SearchRequest {
        public string Query { get; set; } = "";
        public int Limit { get; set; } = 100;
    }
}
