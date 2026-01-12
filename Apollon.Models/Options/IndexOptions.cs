namespace Apollon.Core.Options {
    public class IndexOptions {
        public int NGramSize { get; set; } = 3;
        public HashSet<string> StopWords { get; set; } = ["string"];
    }
}