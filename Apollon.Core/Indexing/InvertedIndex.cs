using Apollon.Core.Analysis;
using Apollon.Core.Documents;

namespace Apollon.Core.Indexing {
    internal class InvertedIndex {
        private readonly Dictionary<string, List<Posting>> _invertedIndex = new();

        public void AddDocument(SearchDocument doc) {
            string[] tokens = Tokenizer.Tokenize(doc.Title)
                .Concat(Tokenizer.Tokenize(doc.Text))
                .Concat(doc.Tags)
                .ToArray();
            
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
    }
}