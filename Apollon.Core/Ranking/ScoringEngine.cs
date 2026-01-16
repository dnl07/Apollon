using Apollon.Core.Documents;
using Apollon.Core.Indexing;
using Apollon.Core.Options;

namespace Apollon.Core.Ranking {
    public class ScoringEngine {
        public Dictionary<Guid, double> ScoreDocuments(
            List<(string token, double boost)> expanded,
            InvertedIndex _invertedIndex,
            DocumentStore docs,
            QueryOptions options) {

            var scores = new Dictionary<Guid, double>();

            foreach ((string term, double boost) in expanded) {
                var postings = _invertedIndex.GetSortedPostings(term);
                int df = postings.Count;

                foreach (var posting in postings) {
                    double bm25 = ComputeBM25Score(posting, df, docs, options);

                    double fieldWeight = posting.Field switch {
                        Field.Title => options.TitleWeight,
                        Field.Description => options.DescriptionWeight,
                        Field.Tags => options.TagWeight,
                        _ => 1.0
                    };

                    double finalScore = bm25 * boost * fieldWeight;

                    if (scores.TryGetValue(posting.DocumentId, out var currentScore)) {
                        scores[posting.DocumentId] = currentScore + finalScore;
                    } else {
                        scores[posting.DocumentId] = finalScore;
                    }
                }
            }
            return scores;
        }

        /// <summary>
        /// Calculates the BM25-Score for a posting given all documents.
        /// </summary>
        private double ComputeBM25Score(Posting posting, int df, DocumentStore docs, QueryOptions options) {
            int n = docs.Count;
            var field = posting.Field;

            // Average length of the field
            double avdl = docs.GetAverageFieldLength(field);

            var tf = posting.TermFrequency;
            var dl = docs.GetLength(posting.DocumentId, posting.Field);

            double bm25 = BM25.ComputeScore(tf, df, n, dl, avdl, options.BM25K, options.BM25B);
            return bm25;
        }
    }
}
