namespace Apollon.Api.Models {
    public class SearchRequest {
        public string Query { get; set; } = "";
        public int MaxDocs { get; set; } = 100;
    }
}
