namespace Apollon.Core.Analysis {
    internal static class Tokenizer {
        public static string[] Tokenize(string text) {
            return text.Split(" ");
        } 
    }
}