using Apollon.Core.Analysis;
using Apollon.Core.Documents;
using Apollon.Core.Fuzzy;
using Apollon.Core.Indexing;
using Apollon.Core.Options;
using Apollon.Models.Search;
using System.Diagnostics;

namespace Apollon.Core.Search {
    public class SearchEngine {
        private readonly IndexOptions _options;

        private readonly DocumentStore _docs = new();
        private readonly TokenRegistry _tokens = new();

        private readonly InvertedIndex _invertedIndex = new();
        private readonly NGramIndex _nGramIndex = null!;

        private readonly FuzzyMatcher _fuzzyMatcher = null!;

        private int _counter = 0;

        public SearchEngine(IndexOptions? options = null) {
            _options = options == null ? new IndexOptions() : options;
            _nGramIndex = new NGramIndex(_options.NGramSize);
            _fuzzyMatcher = new FuzzyMatcher(_nGramIndex, _options);
        }

        public void Initialize(IndexOptions options) {
        }

        public SearchDocument AddDocument(SearchDocument doc) {
            doc.Id = Guid.NewGuid();

            var tokens = DocumentUtils.GetTokensOfDocument(doc, _options.StopWords);

            _docs.Add(doc);
            _invertedIndex.AddDocument(doc, tokens);

            foreach (string token in tokens) {
                if (_tokens.Add(token)) {
                    _nGramIndex.AddToken(token);
                }
            }

            if (_counter % 1000 == 0) Debug.WriteLine(_counter);

            _counter++;

            return doc;
        }

        public SearchResult Search(string request, QueryOptions options) {
            var result = new SearchResult();
            result.Query = request;

            // fuzzy string matching
            var expanded = new List<(string term, double boost)>();

            foreach (string term in Tokenizer.Tokenize(request)) {
                expanded.Add((term, 1.0));
                result.UsedTokens.Add(term);

                foreach (var fuzzy in _fuzzyMatcher.Match(term, options)) {
                    double boost = Math.Exp(-fuzzy.EditDistance);
                    expanded.Add((fuzzy.Token, boost));

                    result.UsedTokens.Add(fuzzy.Token);
                }
            }

            if (expanded.Count == 0) return result;

            var lists = expanded
                .Select(t => _invertedIndex.GetSortedPostings(t.term))
                .Where(l => l.Count > 0)
                .OrderBy(l => l.Count)
                .ToList();

            foreach (var list in lists) {
                SearchUtils.ComputeBM25Scores(list, _docs);
            }

            var merged = lists[0];
            for (int i = 1; i < lists.Count; i++) {
                merged = SearchUtils.MergePostingsLists(merged, lists[i]);
            }

            result.Documents = merged.
                OrderBy(m => m.BM25Score)
                .Select(d => _docs.Get(d.DocumentId))
                .Take(options.MaxDocs)
                .ToList();

            return result;
        }
    }
}