using Apollon.Core.Indexing;

namespace Apollon.Core.Documents {
    public class DocumentStore {
        private readonly Dictionary<Guid, SearchDocument> _docs = new();

        private int _totalDocs;

        public void Add(SearchDocument doc) {
            _docs[doc.Id] = doc;
            _totalDocs++;
            AverageDocumentLength = (double)_totalDocs / _docs.Count;
        }

        public SearchDocument Get(Guid id) {
            return _docs[id];
        }

        public int GetLength(Guid id, Field field) {
            var d = _docs[id];

            return field switch {
                Field.Title => d.Title.Length,
                Field.Description => d.Description.Length,
                Field.Tags => d.Tags.Length,
                _ => 0,
            };
        }

        public int Count => _docs.Count;

        public double AverageDocumentLength { get; private set; }
    }
}