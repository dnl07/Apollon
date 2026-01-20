using Apollon.Core.Documents;

namespace Apollon.Models.Search {
    public class SearchStatus {
        public bool IsRunning { get; set; } = false;
        public DateTime StartetAt { get; set; } = DateTime.Now;
        public int TotalDocuments { get; set; } = 0;
        public int TotalTokens { get; set; } = 0;
        public int IndexSize { get; set; } = 0;
    }
}
