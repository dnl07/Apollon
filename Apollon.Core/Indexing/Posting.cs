namespace Apollon.Core.Indexing {
    public enum Field {
        Title,
        Description,
        Tags
    }

    public class Posting {
        public Guid DocumentId { get; }
        public int TermFrequency { get; set; }
        public Field Field { get; set; }

        public Posting(Guid id, int termFrequency, Field field) {
            DocumentId = id;
            TermFrequency = termFrequency;
            Field = field;
        }
    }
}