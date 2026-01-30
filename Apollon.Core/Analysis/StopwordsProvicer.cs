using Apollon.Core.Options;

namespace Apollon.Core.Analysis {
    public static class StopwordsProvider {
        public static HashSet<string> Default = Load();

        public static HashSet<string> ResolveStopwords(IndexOptions options) {
            return options.StopwordsSource switch {
                StopwordsSource.Default => Default,
                StopwordsSource.Custom => options.StopWords,
                StopwordsSource.DefaultAndCustom => Default.Union(options.StopWords).ToHashSet(),
                _ => Default
            };
        }

        private static HashSet<string> Load() {
            using var stream = typeof(StopwordsProvider)
                .Assembly
                .GetManifestResourceStream("Apollon.Core.Resources.stopwords_en.tsv");

            if (stream == null) return [];

            using var reader = new StreamReader(stream);

            var tokenized = Tokenizer.Tokenize(reader.ReadToEnd());
             
            return tokenized
                .ToHashSet();
        }
    }
}