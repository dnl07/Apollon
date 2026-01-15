using Apollon.Core.Documents;

namespace Apollon.Core.Indexing {
    public class InvertedIndex {
        private readonly Dictionary<string, List<Posting>> _invertedIndex = new();

        public void AddDocument(SearchDocument doc, HashSet<string> tokens) {            
            foreach (string token in tokens) {
                if (!_invertedIndex.TryGetValue(token, out var postings)) {
                    postings = new List<Posting>();
                    _invertedIndex[token] = postings;
                } 
                postings.Add(new Posting(doc.Id, 1));
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