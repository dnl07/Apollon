namespace Apollon.Core.Documents {
    internal class DocumentStore {
        private readonly Dictionary<int, SearchDocument> _docs = new();

        public void Add(SearchDocument doc) {
            _docs[doc.Id] = doc;

            int sum = 0;
            foreach (var id in _docs.Keys) {
               sum += GetLength(id);
            }
            AverageDocumentLength = (double)sum / _docs.Count;
        }

        public SearchDocument Get(int id) {
            return _docs[id];
        }

        public int GetLength(int id) {
            var d = _docs[id];
            return d.Title.Length + d.Text.Length + d.Tags.Length;
        }

        public int Count => _docs.Count;

        public double AverageDocumentLength { get; private set; }
    }
}