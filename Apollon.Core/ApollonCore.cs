using Apollon.Core.Documents;
using Apollon.Core.Search;

namespace Apollon.Core {
    public class ApollonCore {
        private readonly SearchEngine _searchEngine;

        public ApollonCore() {
            _searchEngine = new SearchEngine();
        }

        public void AddDocument(SearchDocument doc) {
            _searchEngine.AddDocument(doc);
        }
    }
}
