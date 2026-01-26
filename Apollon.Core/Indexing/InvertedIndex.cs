using Apollon.Core.Documents;
using Apollon.Models.Indexing;

namespace Apollon.Core.Indexing {
    public class InvertedIndex {
        private readonly Dictionary<string, List<Posting>> _invertedIndex = new();

        public int Count => _invertedIndex.Count;

        public void AddDocument(SearchDocument doc) {
            AddTokens(doc, Field.Title);
            AddTokens(doc, Field.Description);
            AddTokens(doc, Field.Tags);
        }

        public void UpdateDocument(SearchDocument oldDoc, SearchDocument newDoc) {
            RemoveDocument(oldDoc);
            AddDocument(newDoc);
        }

        public void RemoveDocument(SearchDocument doc) {
            RemoveTokens(doc, Field.Title);
            RemoveTokens(doc, Field.Description);
            RemoveTokens(doc, Field.Tags);         
        }

        private void AddTokens(SearchDocument doc, Field field) {
            foreach (string token in doc.GetFieldTokens(field)) {
                if (!_invertedIndex.TryGetValue(token, out var postings)) {
                    postings = new List<Posting>();
                    _invertedIndex[token] = postings;
                }
                postings.Add(new Posting(doc.Id, 1, field));
            }
        }

        private void RemoveTokens(SearchDocument doc, Field field) {
            var tokens = doc.GetFieldTokens(field);

            foreach (var token in tokens) {
                if (_invertedIndex.TryGetValue(token, out var postings)) {
                    postings.RemoveAll( p => 
                    p.DocumentId == doc.Id &&
                    p.Field == field
                    );

                    if (postings.Count == 0) {
                        _invertedIndex.Remove(token);
                    }
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