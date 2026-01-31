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

        /// <summary>
        /// Matches a given token to other token and returns all candidates
        /// </summary>
        public IReadOnlyList<FuzzyToken> Match(string token, TokenRegistry tokenRegistry, QueryOptions queryOptions) {
            // Dictionary to count how many n-Grams each candidate token shares with the input token
            var candidateCounts = new Dictionary<string, int>();

            // Generate n-Grams for the input token and find all possible candidates
            foreach (var nGram in NGramGenerator.Generate(token, _indexOptions.NGramSize)) {
                foreach (var candidateId in _nGramIndex.GetCandidates(nGram)) {
                    var candidateToken = tokenRegistry.GetToken(candidateId);

                    if (candidateToken == token) continue;

                    if (candidateCounts.TryGetValue(candidateToken, out var count)) {
                        candidateCounts[candidateToken] = count + 1;
                    } else {
                        candidateCounts[candidateToken] = 1;
                    }
                }
            }

            // Preselect candidates
            List<string> preselectedTokens = new List<string>();
            var inputNGrams = NGramGenerator.Generate(token, _indexOptions.NGramSize);

            foreach ((string candidateToken, int overlapCount) in candidateCounts) {
                var candidateNGrams = NGramGenerator.Generate(candidateToken, _indexOptions.NGramSize);

                if (overlapCount >= Math.Max(inputNGrams.Count, candidateNGrams.Count) - _indexOptions.NGramSize * queryOptions.MaxEditDistance) {
                    preselectedTokens.Add(candidateToken);
                }
            }

            // Calculate edit distance
            List<FuzzyToken> fuzzyMatches = new List<FuzzyToken>();
            foreach (var preselected in preselectedTokens) {
                var editDistance = EditDistance.Calculate(token, preselected);
                if (editDistance <= queryOptions.MaxEditDistance) {
                    fuzzyMatches.Add(new FuzzyToken(preselected, editDistance));
                }    
            }

            return fuzzyMatches;
        }
    }
}
