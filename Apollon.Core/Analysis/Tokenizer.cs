using System.Runtime.CompilerServices;

namespace Apollon.Core.Analysis {
    public static class Tokenizer {
        public static string[] Tokenize(string text, HashSet<string>? stopWords = null) {
            if (string.IsNullOrWhiteSpace(text)) return Array.Empty<string>();

            var tokens = new HashSet<string>();

            ReadOnlySpan<char> span = text.AsSpan();
            int start = 0;
            for (int i = 0; i <= span.Length; i++) {
                if (i == span.Length || char.IsWhiteSpace(span[i])) {
                    if (i > start) {
                        var token = ProcessToken(span[start..i]);
                        if (!string.IsNullOrWhiteSpace(token)) {
                            tokens.Add(token);
                        }
                    }
                    start = i + 1;
                }
            }

            return tokens.ToArray();
        } 

        private static string ProcessToken(ReadOnlySpan<char> text, HashSet<string>? stopWords = null) {
            var token = Normalizer.Normalize(text);

            if (string.IsNullOrWhiteSpace(token)) return "";

            // Remove stopwords
            if (stopWords != null && stopWords.Contains(token)) return "";

            return token;
        }
    }
}