namespace Apollon.Core.Analysis {
    internal static class NGramGenerator {
        public static IReadOnlyList<string> Generate(string token, int nGramSize, bool prefixEditDistance = false) {
            token = prefixEditDistance ? $"$${token}$$" : $"$${token}";

            List<string> nGrams = new List<string>();

            for (int i = 0; i < token.Length - nGramSize + 1; i++) {
                nGrams.Add(token.Substring(i, nGramSize));
            }

            return nGrams;
        } 
    }
}