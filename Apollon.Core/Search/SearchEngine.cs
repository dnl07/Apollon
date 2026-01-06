using Apollon.Core.Documents;
using Apollon.Core.Indexing;

namespace Apollon.Core.Search {
    internal class SearchEngine {
        private readonly InvertedIndex _invertedIndex = new();

        public void AddDocument(SearchDocument doc) {
            _invertedIndex.AddDocument(doc);
        }
    }
}