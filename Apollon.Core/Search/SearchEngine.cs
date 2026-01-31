using Apollon.Core.Analysis;
using Apollon.Core.Documents;
using Apollon.Core.Fuzzy;
using Apollon.Core.Indexing;
using Apollon.Core.Options;
using Apollon.Core.Ranking;
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

            // TODO: Change init multiple times before adding documents

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

            var stopwords = StopwordsProvider.ResolveStopwords(_options);

            doc.Tokenize(stopwords);

            _docs.Add(doc);
            _invertedIndex.AddDocument(doc);

            var allTokens = doc.AllTokens;

            foreach (string token in allTokens) {
                var id = _tokens.Add(token);

                if (id != -1) {
                    _nGramIndex.AddToken(token, id);
                }
            }

            return doc;
        }

        public void RemoveDocument(Guid docId) {
            var doc = _docs.Get(docId);

            if (doc == null) return;

            _docs.Remove(doc.Id);
            _invertedIndex.RemoveDocument(doc);
            
            foreach (var token in doc.AllTokens) {
                var tokenId = _tokens.GetIdOfToken(token);
                if (tokenId == -1) continue;

                _tokens.Remove(token);
                _nGramIndex.RemoveToken(token, tokenId);
            }
        }

        public void UpdateDocument(Guid oldId, SearchDocument newDoc) {
            RemoveDocument(oldId);
            AddDocument(newDoc);
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

            // uses a min-heap to improve search performance
            var topK = new PriorityQueue<KeyValuePair<Guid, ScoreResult>, double>();

            foreach (var kv in scores) {
                double score = kv.Value.FinalScore;
                if (topK.Count < options.MaxDocs) {
                    topK.Enqueue(kv, score);
                } else if (score > topK.Peek().Value.FinalScore) {
                    topK.Dequeue();
                    topK.Enqueue(kv, score);
                }
            }

            var topKHits = topK.UnorderedItems
                .OrderByDescending(k => k.Priority)
                .Select(s => s.Element);

            foreach (var (id, scoreResult) in topKHits) {
                var doc = _docs.Get(id);

                if (doc == null) continue;

                result.Hits.Add(new SearchHit {
                    Document = doc,
                    Explain = scoreResult
                });
            }

            // TODO: Better Take() performance
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