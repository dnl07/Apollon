namespace Apollon.Core.Options {
    public class QueryOptions {
        public int MaxDocs { get; set; } = 5;
        public int MaxEditDistance { get; set; } = 2;
        public int MaxPrefixEditDistance { get; set; } = 1;
    }
}