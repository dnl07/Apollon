using Apollon.Core.Analysis;

namespace Apollon.Core.Documents {
    public static class DocumentUtils {
        public static HashSet<string> GetTokensOfDocument(SearchDocument doc, HashSet<string>? stopWords = null) {
            return Tokenizer.Tokenize(doc.Title, stopWords)
                .Concat(Tokenizer.Tokenize(doc.Text, stopWords))
                .Concat(doc.Tags)
                .ToHashSet();
        }
    }
}