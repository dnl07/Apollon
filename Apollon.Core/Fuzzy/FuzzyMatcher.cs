using Apollon.Core.Analysis;
using Apollon.Core.Indexing;
using Apollon.Core.Options;

namespace Apollon.Core.Fuzzy {
    internal class FuzzyMatcher {
        private readonly NGramIndex _nGramIndex;
        private readonly IndexOptions _indexOptions;

        public FuzzyMatcher(NGramIndex nGramIndex, IndexOptions options) {
            _nGramIndex = nGramIndex;
            _indexOptions = options;
        }

        public IReadOnlyList<FuzzyToken> Match(string token, QueryOptions queryOptions) {
            var candidates = new HashSet<string>();

            foreach (var nGram in NGramGenerator.Generate(token, _indexOptions.NGramSize)) {
                foreach (var candidate in _nGramIndex.GetCandidates(nGram)) {
                    candidates.Add(candidate);
                }
            }

            List<FuzzyToken> tokens = new List<FuzzyToken>();

            foreach (var candidate in candidates) {
                var editDistance = EditDistance.Calculate(token, candidate);
                if (editDistance <= queryOptions.MaxEditDistance) {
                    tokens.Add(new FuzzyToken(token, editDistance));
                }    
            }

            return tokens;
        }
    }
}
