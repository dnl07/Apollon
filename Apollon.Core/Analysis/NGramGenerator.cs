namespace Apollon.Core.Analysis {
    public static class NGramGenerator {
        public static IReadOnlyList<string> Generate(string token, int nGramSize, bool prefixEditDistance = false) {
            if(string.IsNullOrWhiteSpace(token)) return [];

            string edgeSymbols = string.Join("", Enumerable.Repeat("$", nGramSize -1));

            var paddedToken = prefixEditDistance ?  $"{edgeSymbols}{token}" : $"{edgeSymbols}{token}{edgeSymbols}";

            List<string> nGrams = new List<string>(paddedToken.Length - nGramSize + 1);

            ReadOnlySpan<char> span = paddedToken.AsSpan();

            for (int i = 0; i < span.Length - nGramSize + 1; i++) {
                nGrams.Add(span.Slice(i, nGramSize).ToString());
            }

            return nGrams;
        } 
    }
}