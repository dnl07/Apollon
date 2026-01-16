namespace Apollon.Core.Analysis {
    internal static class Tokenizer {
        public static string[] Tokenize(string text, HashSet<string>? stopWords = null) {
            if (string.IsNullOrWhiteSpace(text)) return Array.Empty<string>();

            var rawTokens = text.Split(
                [" ", "\n", "\r", "\t"],
                StringSplitOptions.RemoveEmptyEntries
            );

            var tokens = new List<string>();

            foreach (var raw in rawTokens) {
                var token = Normalizer.Normalize(raw);

                // Remove stopwords
                if (stopWords != null && stopWords.Contains(token)) continue;

                tokens.Add(token);
            }

            return tokens.ToArray();
        } 
    }
}