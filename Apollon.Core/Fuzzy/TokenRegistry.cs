namespace Apollon.Core.Documents {
    public class TokenRegistry {
        private readonly Dictionary<Guid, string> _idToToken = new Dictionary<Guid, string>();
        private readonly Dictionary<string, Guid> _tokenToId = new Dictionary<string, Guid>();

        public void Add(string token) {
            Guid id = Guid.NewGuid();

            _idToToken[id] = token;
            _tokenToId[token] = id;
        }

        public string GetToken(Guid id) {
            return _idToToken[id];
        }

        public Guid GetIdOfToken(string token) {
            return _tokenToId[token];
        }
    }
}
