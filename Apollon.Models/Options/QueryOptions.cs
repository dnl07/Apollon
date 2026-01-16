namespace Apollon.Core.Options {
    public class QueryOptions {
        public int MaxDocs { get; set; } = 5;

        // Fuzzy search
        public int MaxEditDistance { get; set; } = 2;
        public int MaxPrefixEditDistance { get; set; } = 1;
        public int EditDistanceLimit { get; set; } = 10;

        // Scoring
        public double BM25K { get; set; } = 1.75;
        public double BM25B { get; set; } = 0.75;
        public double TitleWeight { get; set; } = 3.0;
        public double DescriptionWeight { get; set; } = 1.0;
        public double TagWeight { get; set; } = 2.0;

    }
}