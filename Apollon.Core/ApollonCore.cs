using Apollon.Core.Documents;
using Apollon.Core.Search;

namespace Apollon.Core {
    public class ApollonCore {
        public readonly SearchEngine SearchEngine;

        public ApollonCore() {
            SearchEngine = new SearchEngine();
        }
    }
}
