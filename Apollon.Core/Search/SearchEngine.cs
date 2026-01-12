using Apollon.Core.Analysis;
using Apollon.Core.Documents;
using Apollon.Core.Fuzzy;
using Apollon.Core.Indexing;
using Apollon.Core.Options;
using Apollon.Models.Search;

namespace Apollon.Core.Search {
    public class SearchEngine {
        private IndexOptions _options = new();
        private bool _initialized = false;

        private readonly DocumentStore _docs = new();
        private readonly TokenRegistry _tokens = new();

        private readonly InvertedIndex _invertedIndex = new();
        private NGramIndex _nGramIndex = null!;

        private FuzzyMatcher _fuzzyMatcher = null!;

        public void Initialize(IndexOptions options) {
            if (_initialized) {
                throw new InvalidOperationException("SearchEngine is already initialized.");
            }

            _options = options;
            _nGramIndex = new NGramIndex(_options.NGramSize);
            _fuzzyMatcher = new FuzzyMatcher(_nGramIndex, _options);

            _initialized = true;
        }

        private void EnsureInitialized() {
            if (!_initialized) {
                throw new InvalidOperationException("SearchEngine is not initialized.");
            }
        }

        public SearchDocument AddDocument(SearchDocument doc) {
            EnsureInitialized();

            doc.Id = Guid.NewGuid();

            var tokens = DocumentUtils.GetTokensOfDocument(doc, _options.StopWords);

            _docs.Add(doc);
            _invertedIndex.AddDocument(doc, tokens);

            foreach (string token in tokens) {
                if (_tokens.Add(token)) {
                    _nGramIndex.AddToken(token);
                }
            }
            return doc;
        }

        public SearchResult Search(string request, QueryOptions options) {
            EnsureInitialized();

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

            List<List<Posting>> invertedLists = new();
            foreach ((string term, double boost) in expanded) {
                var postings = _invertedIndex.GetSortedPostings(term);

                foreach (var posting in postings) {
                    posting.Score = SearchUtils.ComputeBM25Score(posting, postings.Count, _docs);
                    posting.Score *= boost;
                }

                invertedLists.Add(postings);
            }
            invertedLists = invertedLists.OrderBy(l => l.Count).ToList();

            var merged = invertedLists[0];
            for (int i = 1; i < invertedLists.Count; i++) {
                merged = SearchUtils.MergePostingsLists(merged, invertedLists[i]);
            }

            result.Documents = merged.
                OrderBy(m => m.Score)
                .Select(d => _docs.Get(d.DocumentId))
                .Take(options.MaxDocs)
                .ToList();

            return result;
        }
    }
}