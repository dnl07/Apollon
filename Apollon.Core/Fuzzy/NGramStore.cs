namespace Apollon.Core.Fuzzy {
    public class NGramStore {
        private readonly Dictionary<int, string> _idToNGram= new Dictionary<int, string>();
        private readonly Dictionary<string, int> _nGramToId = new Dictionary<string, int>();

        private int _idCounter = 0;

        public int Count => _idToNGram.Count;

        public string GetNGram(int id) {
            if (_idToNGram.TryGetValue(id, out var nGram)) {
                return nGram;
            }
            return "";
        }

        public int GetOrAddId(string nGram) {
            if (_nGramToId.TryGetValue(nGram, out var id)) {
                return id;
            } else {
                return AddNGram(nGram);
            }
        }

        private int AddNGram(string nGram) {
            var id = _idCounter;
            _idCounter++;

            _idToNGram.Add(id, nGram);
            _nGramToId.Add(nGram, id);
            
            return id;
        }

        public void Remove(string nGram) {

        }
    }
}