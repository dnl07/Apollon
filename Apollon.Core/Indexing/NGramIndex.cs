using Apollon.Core.Analysis;

namespace Apollon.Core.Indexing {
    public class NGramIndex {
        private readonly Dictionary<string, HashSet<int>> _nGramIndex = new();

        private int _nGramSize { get; }

        public NGramIndex(int nGramSize) {
            _nGramSize= nGramSize;
        }

        public void AddToken(string token, int id) {
            foreach (var nGram in NGramGenerator.Generate(token, _nGramSize)) {
                if (_nGramIndex.TryGetValue(nGram, out var tokenIds)) {
                    tokenIds.Add(id);
                } else {
                    _nGramIndex[nGram] = [id];
                }
            }
        }

        public void RemoveToken(string token, int id) {
            foreach (var nGram in NGramGenerator.Generate(token, _nGramSize)) {
                if (_nGramIndex.TryGetValue(nGram, out var ids)) {
                    ids.Remove(id);
                    if (ids.Count == 0) _nGramIndex.Remove(nGram);
                }
            }
        }

        public HashSet<int> GetCandidates(string nGram) {
            if (_nGramIndex.TryGetValue(nGram, out var candidates)) {  
                return candidates ?? []; 
            }
            return [];
        }
    }
}