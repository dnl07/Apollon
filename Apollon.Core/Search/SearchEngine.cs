using Apollon.Core.Analysis;
using Apollon.Core.Documents;
using Apollon.Core.Indexing;

namespace Apollon.Core.Search {
    public class SearchEngine {
        private readonly DocumentStore _docs = new();
        private readonly InvertedIndex _invertedIndex = new();

        public SearchDocument AddDocument(SearchDocument doc) {
            doc.Id = Guid.NewGuid();

            _docs.Add(doc);
            _invertedIndex.AddDocument(doc);

            return doc;
        }

        public List<SearchDocument> Search(string request, int maxDocs) {
            string[] terms = Tokenizer.Tokenize(request);

            if (terms.Length == 0) return [];

            var lists = terms
                .Select(t => _invertedIndex.GetSortedPostings(t))
                .OrderBy(l => l.Count)
                .ToList();

            foreach (var list in lists) {
                SearchUtils.ComputeBM25Scores(list, _docs);
            }

            var merged = lists[0];
            for (int i = 1; i < lists.Count; i++) {
                merged = SearchUtils.MergePostingsLists(merged, lists[i]);
            }
            return merged.
                OrderBy(m => m.BM25Score)
                .Select(d => _docs.Get(d.DocumentId))
                .Take(maxDocs)
                .ToList();
        }
    }
}