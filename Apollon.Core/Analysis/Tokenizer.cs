namespace Apollon.Core.Analysis {
    internal static class Tokenizer {
        public static string[] Tokenize(string text, HashSet<string>? stopWords = null) {
            var tokens = Normalizer.Normalize(text).Split(" ");

            // Remove stopwords
            if (stopWords != null) {
                _ = tokens.Select(stopWords.Contains);
            }

            return tokens;
        } 
    }
}