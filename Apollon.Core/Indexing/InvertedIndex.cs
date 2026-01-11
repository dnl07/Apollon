using Apollon.Core.Documents;

namespace Apollon.Core.Indexing {
    internal class InvertedIndex {
        private readonly Dictionary<string, List<Posting>> _invertedIndex = new();

        public void AddDocument(SearchDocument doc) {
            string[] tokens = DocumentUtils.GetTokensOfDocument(doc);
            
            foreach (string token in tokens) {
                if (_invertedIndex.TryGetValue(token, out var postings)) {
                    int index = postings.FindIndex(p => p.DocumentId == doc.Id);

                    if (index == -1) {
                        postings.Add(new Posting(doc.Id, 1));
                    } else {
                        postings[index].TermFrequency++;
                    }
                } else {
                    _invertedIndex[token] = [new Posting(doc.Id, 1)];
                }
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