using Apollon.Core.Analysis;
using Apollon.Models.Indexing;

namespace Apollon.Core.Documents {
    public class DocumentStore {
        private readonly Dictionary<Guid, SearchDocument> _docs = new();

        private int _totalDocs;
        private readonly Dictionary<Field, long> _totalFieldLengths = new();
        private readonly Dictionary<Field, int> _docCountsPerField = new();

        public DocumentStore() {
            foreach (Field field in Enum.GetValues(typeof(Field))) {
                _totalFieldLengths[field] = 0;
                _docCountsPerField[field] = 0;
            }
        }

        /// <summary>
        /// Adds a document and updates field statistics.
        /// </summary>
        public void Add(SearchDocument doc) {
            _docs[doc.Id] = doc;
            _totalDocs++;

            UpdateFieldLengths(doc.Title, Field.Title);
            UpdateFieldLengths(doc.Description, Field.Description);
            UpdateFieldLengths(string.Join(" ", doc.Tags), Field.Tags);
        }

        private void UpdateFieldLengths(string? text, Field field) {
            if (string.IsNullOrWhiteSpace(text)) return;

            var tokens = Tokenizer.Tokenize(text);
            if (tokens.Length == 0) return;

            _totalFieldLengths[field] += tokens.Length;
            _docCountsPerField[field]++;
        }

        public SearchDocument Get(Guid id) {
            return _docs[id];
        }

        public int GetLength(Guid id, Field field) {
            if (!_docs.TryGetValue(id, out var doc)) return 0;

            string text = field switch {
                Field.Title => doc.Title,
                Field.Description => doc.Description,
                Field.Tags => string.Join(" ", doc.Tags),
                _ => "",
            };

            if (string.IsNullOrWhiteSpace(text)) return 0;

            return Tokenizer.Tokenize(text).Length;
        }

        public int Count => _docs.Count;

        /// <summary>
        /// Returns the average field length in tokens.
        /// </summary>
        public double GetAverageFieldLength(Field field) {
            int docCount = _docCountsPerField[field];
            if (docCount == 0) return 0;

            return (double)_totalFieldLengths[field] / docCount;
        }
    }
}