namespace Apollon.Core.Indexing {
    public class Posting {
        public Guid DocumentId { get; }
        public int TermFrequency { get; set; }
        public double BM25Score { get; set; } = 0;

        public Posting(Guid id, int termFrequency) {
            DocumentId = id;
            TermFrequency = termFrequency;
        }
    }
}