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
            var candidates = new HashSet<int>();

            foreach (var nGram in NGramGenerator.Generate(token, _indexOptions.NGramSize)) {
                candidates = candidates.Concat(_nGramIndex.GetCandidates(nGram)).ToHashSet();
            }

            List<FuzzyToken> tokens = new List<FuzzyToken>();

            foreach (var id in candidates) {
                string candidate = tokenRegistry.GetToken(id);

                var editDistance = EditDistance.Calculate(token, candidate);
                if (editDistance <= queryOptions.MaxEditDistance) {
                    tokens.Add(new FuzzyToken(candidate, editDistance));
                }    
            }

            return tokens;
        }
    }
}
