using Apollon.Core.Analysis;
using Apollon.Core.Indexing;

namespace Apollon.Core.Documents {
    public static class DocumentUtils {
        public static HashSet<string> GetTokensOfDocumentField(SearchDocument doc, Field field, HashSet<string>? stopWords = null) {
            var relevantText = field switch {
                Field.Title => doc.Title,
                Field.Description => doc.Description,
                Field.Tags => string.Join(" ", doc.Tags ?? Array.Empty<string>()),
                _ => ""
            };

            return Tokenizer.Tokenize(relevantText, stopWords).ToHashSet();
        }
    }
}