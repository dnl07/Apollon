using Apollon.Core.Analysis;

namespace Apollon.Core.Indexing {
    internal class NGramIndex {
        private readonly Dictionary<string, List<string>> _nGramIndex = new();

        private int _nGramSize { get; }

        public NGramIndex(int nGramSize) {
            _nGramSize= nGramSize;
        }

        public void AddToken(string token) {
            foreach (var nGram in NGramGenerator.Generate(token, _nGramSize)) {
                if (_nGramIndex.TryGetValue(nGram, out var tokens)) {
                    if (!tokens.Contains(token)) {
                        tokens.Add(token);
                    }
                } else {
                    _nGramIndex[nGram] = [token];
                }
            }
        }

        public IReadOnlyList<string> GetCandidates(string token) {
            if (_nGramIndex.TryGetValue(token, out var candidates)) {  
                return candidates; 
            }

            return [];
        }
    }
}