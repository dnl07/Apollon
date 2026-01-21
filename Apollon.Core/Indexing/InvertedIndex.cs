using Apollon.Core.Documents;
using Apollon.Models.Indexing;

namespace Apollon.Core.Indexing {
    public class InvertedIndex {
        private readonly Dictionary<string, List<Posting>> _invertedIndex = new();

        public int Count => _invertedIndex.Count;

        public void AddDocument(SearchDocument doc, HashSet<string> titleTokens, HashSet<string> descriptionTokens, HashSet<string> tagTokens) {
            AddTokens(doc, titleTokens, Field.Title);
            AddTokens(doc, descriptionTokens, Field.Description);
            AddTokens(doc, tagTokens, Field.Tags);
        }

        private void AddTokens(SearchDocument doc, HashSet<string> tokens, Field field) {
            foreach (string token in tokens) {
                if (!_invertedIndex.TryGetValue(token, out var postings)) {
                    postings = new List<Posting>();
                    _invertedIndex[token] = postings;
                }
                postings.Add(new Posting(doc.Id, 1, field));
            }
        }

        public List<Posting> GetSortedPostings(string term) {
            if (_invertedIndex.TryGetValue(term, out var postings)) {
                postings.Sort((a, b) => a.DocumentId.CompareTo(b.DocumentId));
                return postings;
            } 
            return [];
        }
    }
}