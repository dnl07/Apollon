using Apollon.Models.Fuzzy;

namespace Apollon.Core.Documents {
    public class TokenRegistry {
        private readonly Dictionary<Guid, FuzzyWord> _words = new Dictionary<Guid, FuzzyWord>();

        public void Add(FuzzyWord word) {
            _words[word.Id] = word;
        }

        public FuzzyWord Get(Guid id) {
            return _words[id];
        }
    }
}
