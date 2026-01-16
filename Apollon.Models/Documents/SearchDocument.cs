namespace Apollon.Core.Documents {
    public class SearchDocument {
        public Guid Id { get; set; } = default!;
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string[] Tags { get; set; } = [];
    }
}