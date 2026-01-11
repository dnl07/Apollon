namespace Apollon.Core.Options {
    public class SearchOptions {
        public int NGramsSize { get; set; } = 3;
        public int MaxEditDistance { get; set; } = 2;
        public int MaxPrefixEditDistance { get; set; } = 1;
        public HashSet<string> StopWords { get; set; } = [];
    }
}