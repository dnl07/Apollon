namespace Apollon.Core.Documents {
    internal class DocumentStore {
        private readonly Dictionary<int, SearchDocument> _docs = new();

        public void Add(SearchDocument doc) {
            _docs[doc.Id] = doc;
        }

        public SearchDocument Get(int id) {
            return _docs[id];
        }

        public int Count => _docs.Count;
    }
}