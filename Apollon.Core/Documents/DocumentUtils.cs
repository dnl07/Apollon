using Apollon.Core.Analysis;

namespace Apollon.Core.Documents {
    public static class DocumentUtils {
        public static HashSet<string> GetTokensOfDocument(SearchDocument doc) {
            return Tokenizer.Tokenize(doc.Title)
                .Concat(Tokenizer.Tokenize(doc.Text))
                .Concat(doc.Tags)
                .ToHashSet();
        }
    }
}