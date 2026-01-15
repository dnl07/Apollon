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
                var id = _tokens.Add(token);

                if (id != -1) {
                    _nGramIndex.AddToken(token, id);
                }
            }

            return doc;
        }

        public SearchResult Search(string request, QueryOptions options) {
            EnsureInitialized();

            var result = new SearchResult();
            result.Query = request;

            // fuzzy string matching
            var expanded = SearchUtils.FuzzySearch(request, _fuzzyMatcher, _tokens, options);
            // inverted Lists
            Dictionary<Guid, double> scores = SearchUtils.CreateScores(expanded, _invertedIndex, _docs);

            result.Documents = scores.
                OrderByDescending(d => d.Value)
                .Take(options.MaxDocs)
                .Select(d => _docs.Get(d.Key))
                .ToList();

            return result;
        }
    }
}