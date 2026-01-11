namespace Apollon.Models.Api {
    public class SearchRequest {
        public string Query { get; set; } = "";
        public int MaxDocs { get; set; } = 100;
    }
}
