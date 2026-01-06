namespace Apollon.Core.Indexing {
    internal class Posting {
        public int DocumentId { get; }
        public int TermFrequency { get; set; }

        public Posting(int id, int termFrequency) {
            DocumentId = id;
            TermFrequency = termFrequency;
        }
    }
}