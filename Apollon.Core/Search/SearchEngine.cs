using Apollon.Core.Documents;
using Apollon.Core.Fuzzy;
using Apollon.Core.Indexing;
using Apollon.Core.Options;
using Apollon.Core.Ranking;
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
        private QueryExpander _expander = null!;
        private ScoringEngine _scoring = null!;

        public void Initialize(IndexOptions options) {
            if (_initialized) {
                throw new InvalidOperationException("SearchEngine is already initialized.");
            }

            _options = options;
            _nGramIndex = new NGramIndex(_options.NGramSize);
            _fuzzyMatcher = new FuzzyMatcher(_nGramIndex, _options);
            _expander = new QueryExpander();
            _scoring = new ScoringEngine();

            _initialized = true;
        }

        private void EnsureInitialized() {
            if (!_initialized) {
                throw new InvalidOperationException("SearchEngine is not initialized.");
            }
        }

        public SearchDocument AddDocument(SearchDocument doc) {
            if (!_initialized) {
                Initialize(new IndexOptions());
                _initialized = true;
            }

            doc.Id = Guid.NewGuid();

            var titleTokens = DocumentUtils.GetTokensOfDocumentField(doc, Field.Title, _options.StopWords);
            var descTokens = DocumentUtils.GetTokensOfDocumentField(doc, Field.Description, _options.StopWords);
            var tagTokens = DocumentUtils.GetTokensOfDocumentField(doc, Field.Tags, _options.StopWords);

            _docs.Add(doc);
            _invertedIndex.AddDocument(doc, titleTokens, descTokens, tagTokens);

            var allTokens = new HashSet<string>(titleTokens);
            allTokens.UnionWith(descTokens);
            allTokens.UnionWith(tagTokens);

            foreach (string token in allTokens) {
                var id = _tokens.Add(token);

                if (id != -1) {
                    _nGramIndex.AddToken(token, id);
                }
            }

            return doc;
        }

        public SearchResult Search(string request, QueryOptions? options = null) {
            EnsureInitialized();

            options ??= new QueryOptions();

            var result = new SearchResult();
            result.Query = request;

            // fuzzy string matching
            var expanded = _expander.Expand(request, _fuzzyMatcher, _tokens, options);

            result.UsedTokens = expanded.Select(e => e.token).ToList();

            // creates scores
            Dictionary<Guid, double> scores = _scoring.ScoreDocuments(expanded, _invertedIndex, _docs, options);

            result.Documents = scores.
                OrderByDescending(d => d.Value)
                .Take(options.MaxDocs)
                .Select(d => _docs.Get(d.Key))
                .ToList();

            return result;
        }
    }
}