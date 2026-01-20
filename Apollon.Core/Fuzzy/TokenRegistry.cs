namespace Apollon.Core.Documents {
    public class TokenRegistry {
        private readonly Dictionary<int, string> _idToToken = new Dictionary<int, string>();
        private readonly Dictionary<string, int> _tokenToId = new Dictionary<string, int>();

        private int _idCounter = 0;

        public int Count => _tokenToId.Count;

        /// <summary>
        /// Adds the token to the N-Gram-Index.
        /// </summary>
        /// <returns>The corresponding id of the token if successfully added, -1 otherwise.</returns>
        public int Add(string token) {
            if (!_tokenToId.ContainsKey(token)) {
                _idToToken.Add(_idCounter, token);
                _tokenToId.Add(token, _idCounter);
                _idCounter++;

                return _idCounter - 1;
            }
            return -1;
        }

        public string GetToken(int id) {
            return _idToToken[id];
        }

        public int GetIdOfToken(string token) {
            return _tokenToId[token];
        }
    }
}