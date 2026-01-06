using Apollon.Core.Analysis;
using Apollon.Core.Documents;
using Apollon.Core.Indexing;
using Apollon.Core.Ranking;

namespace Apollon.Core.Search {
    internal class SearchEngine {
        private readonly DocumentStore _docs = new();
        private readonly InvertedIndex _invertedIndex = new();

        public void AddDocument(SearchDocument doc) {
            _docs.Add(doc);
            _invertedIndex.AddDocument(doc);
        }

        public void Search(string request) {
            string[] terms = Tokenizer.Tokenize(request);

            int n = _docs.Count;
            double avdl = _docs.AverageDocumentLength;
            double k = 1.75;
            double b = 0.75;

            var lists = terms
                .Select(t => _invertedIndex.GetSortedPostings(t))
                .OrderBy(l => l.Count)
                .ToList();

            foreach (var list in lists) {
                ComputeScores(list, n, avdl, k, b);
            }

            var intersected = lists[0];
            for (int i = 1; i < lists.Count; i++) {
                intersected = Intersect(intersected, lists[i]);
            }
        }

        private void ComputeScores(List<Posting> postings, int n, double avdl, double k, double b) {
            foreach (var posting in postings) {
                var tf = posting.TermFrequency;
                var df = postings.Count;
                var dl = _docs.GetLength(posting.DocumentId);

                posting.BM25Score = BM25.ComputeScore(tf, df, n, dl, avdl, k, b);
            }
        }

        private List<Posting> Intersect(List<Posting> a, List<Posting> b) {
            List<Posting> result = [];

            int i = 0;
            int j = 0;

            while (i < a.Count && j < b.Count) {
                if (a[i].DocumentId == b[j].DocumentId) {
                    a[i].TermFrequency += b[j].TermFrequency;
                    result.Add(a[i]);
                    i++;
                } else if (a[i].DocumentId < b[j].DocumentId) {
                    result.Add(a[i]);
                    i++;
                } else {
                    result.Add(b[i]);
                    j++;
                }
            }

            while(i < a.Count) {
                result.Add(a[i]);
                i++;
            }

            while(j < b.Count) {
                result.Add(b[j]);
                j++;
            }

            return result;
        }
    }
}