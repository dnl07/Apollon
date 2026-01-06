namespace Apollon.Models {
    public class SearchDocument {
        public string Id { get; set; } = default!;
        public string Title { get; set; } = "";
        public string Text { get; set; } = "";
        public string[] Tags { get; set; } = [];
    }
}
