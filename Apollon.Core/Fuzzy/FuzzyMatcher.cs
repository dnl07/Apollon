using Apollon.Core.Analysis;
using Apollon.Core.Documents;
using Apollon.Core.Indexing;
using Apollon.Core.Options;

namespace Apollon.Core.Fuzzy {
    public class FuzzyMatcher {
        private readonly NGramIndex _nGramIndex;
        private readonly IndexOptions _indexOptions;

        public FuzzyMatcher(NGramIndex nGramIndex, IndexOptions options) {
            _nGramIndex = nGramIndex;
            _indexOptions = options;
        }

        public IReadOnlyList<FuzzyToken> Match(string token, TokenRegistry tokenRegistry, QueryOptions queryOptions) {
            var candidates = new Dictionary<string, int>();

            foreach (var nGram in NGramGenerator.Generate(token, _indexOptions.NGramSize)) {
                foreach (var candidateId in _nGramIndex.GetCandidates(nGram)) {
                    var candidate = tokenRegistry.GetToken(candidateId);

                    if (candidate == token) continue;

                    if (candidates.TryGetValue(candidate, out var count)) {
                        candidates[candidate] = count + 1;
                    } else {
                        candidates[candidate] = 1;
                    }
                }
            }

            List<FuzzyToken> tokens = new List<FuzzyToken>();

            var cand = new List<string>();
            foreach ((string key, int c) in candidates) {
                var nGramsA = NGramGenerator.Generate(token, _indexOptions.NGramSize);
                var nGramsB = NGramGenerator.Generate(key, _indexOptions.NGramSize);

                if (c >= Math.Max(nGramsA.Count, nGramsB.Count) - _indexOptions.NGramSize * queryOptions.MaxEditDistance)
                        {
                            cand.Add(key);
                        }
            }


            foreach (var id in cand) {
                var editDistance = EditDistance.Calculate(token, id);
                if (editDistance <= queryOptions.MaxEditDistance) {
                    tokens.Add(new FuzzyToken(id, editDistance));
                }    
            }

            return tokens;
        }
    }
}
