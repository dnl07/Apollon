using Apollon.Core.Documents;
using Apollon.Models.Scoring;

namespace Apollon.Models.Search {
    public class SearchHit {
        public SearchDocument Document { get; set; } = new();
        public ScoreResult? Explain { get; set; } = null;
        }
}
