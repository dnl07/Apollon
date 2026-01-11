namespace Apollon.Core.Documents {
    internal class DocumentStore {
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

        public int GetLength(Guid id) {
            var d = _docs[id];
            return d.Title.Length + d.Text.Length + d.Tags.Length;
        }

        public int Count => _docs.Count;

        public double AverageDocumentLength { get; private set; }
    }
}