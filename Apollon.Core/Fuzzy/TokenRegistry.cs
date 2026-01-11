namespace Apollon.Core.Documents {
    public class TokenRegistry {
        private readonly Dictionary<Guid, string> _idToToken = new Dictionary<Guid, string>();
        private readonly Dictionary<string, Guid> _tokenToId = new Dictionary<string, Guid>();

        public bool Add(string token) {
            var id = Guid.NewGuid();
            if (!_tokenToId.TryAdd(token, id)) {
                return false;
            }

            _idToToken[id] = token;
            return true;
        }

        public string GetToken(Guid id) {
            return _idToToken[id];
        }

        public Guid GetIdOfToken(string token) {
            return _tokenToId[token];
        }
    }
}