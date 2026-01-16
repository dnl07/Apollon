namespace Apollon.Core.Analysis {
    public static class NGramGenerator {
        public static IReadOnlyList<string> Generate(string token, int nGramSize, bool prefixEditDistance = false) {
            if(string.IsNullOrWhiteSpace(token)) return [];

            string edgeSymbols = string.Join("", Enumerable.Repeat("$", nGramSize -1));

            token = prefixEditDistance ?  $"{edgeSymbols}{token}" : $"{edgeSymbols}{token}{edgeSymbols}";

            List<string> nGrams = new List<string>();

            for (int i = 0; i < token.Length - nGramSize + 1; i++) {
                nGrams.Add(token.Substring(i, nGramSize));
            }

            return nGrams;
        } 
    }
}