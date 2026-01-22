using Apollon.Core.Documents;
using Apollon.Core.Fuzzy;
using Apollon.Core.Indexing;
using Apollon.Core.Options;
using Apollon.Core.Ranking;
using Apollon.Models.Indexing;
using Apollon.Models.Scoring;
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

        private bool _isRunning = false;
        private DateTime _startedAt;

        public SearchEngine() {
            _isRunning = true;
            _startedAt = DateTime.Now;
        }

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

            var titleTokens = doc.GetTokens(Field.Title, _options.StopWords);
            var descTokens = doc.GetTokens(Field.Description, _options.StopWords);
            var tagTokens = doc.GetTokens(Field.Tags, _options.StopWords);

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

        public SearchResult Search(string request, bool explain, QueryOptions? options = null) {
            EnsureInitialized();
        
            options ??= new QueryOptions();

            var result = new SearchResult();
            result.Query = request;

            // fuzzy string matching
            var expanded = _expander.Expand(request, _fuzzyMatcher, _tokens, options);

            // creates scores
            Dictionary<Guid, ScoreResult> scores = _scoring.ScoreDocuments(expanded, _invertedIndex, _docs, options, explain);

            result.Hits = scores.
                OrderByDescending(d => d.Value.FinalScore)
                .Take(options.MaxDocs)
                .Select(d => new SearchHit {
                    Document = _docs.Get(d.Key),
                    Explain = explain ? d.Value : null
                })
                .ToList();

            return result;
        }

        public SearchStatus GetStatus(bool onlyRunning = false) {
            if (onlyRunning) return new SearchStatus {
              IsRunning = _isRunning  
            };

            return new SearchStatus {
                IsRunning = _isRunning,
                StartetAt = _startedAt,
                TotalDocuments = _docs.Count,
                TotalTokens = _tokens.Count,
                IndexSize = _invertedIndex.Count
            };
        }
    }
}