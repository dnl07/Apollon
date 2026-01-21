using Apollon.Core.Documents;
using Apollon.Core.Indexing;
using Apollon.Core.Options;
using Apollon.Models.Indexing;
using Apollon.Models.Scoring;

namespace Apollon.Core.Ranking {
    public class ScoringEngine {
        public Dictionary<Guid, ScoreResult> ScoreDocuments(
            List<(string token, double boost)> expanded,
            InvertedIndex _invertedIndex,
            DocumentStore docs,
            QueryOptions options,
            bool explain) {

            var scores = new Dictionary<Guid, ScoreResult>();

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

                    if (!scores.TryGetValue(posting.DocumentId, out var score)) {
                        score = new ScoreResult();
                        scores[posting.DocumentId] = score;
                    }

                    score.FinalScore += finalScore;

                    if (explain) {
                        if (!score.Contributions.TryGetValue(term, out var contributions)) {
                            contributions = new List<ScoreContribution>();
                            score.Contributions[term] = contributions; 
                        }

                        contributions.Add(new ScoreContribution {
                           Field = posting.Field,
                           BM25 = bm25,
                           Boost = boost,
                           FieldWeight = fieldWeight,
                           Final = finalScore 
                        });
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

            if (n <= 0 || df <= 0 || df > n) return 0.0;

            // Average length of the field
            double avdl = docs.GetAverageFieldLength(field);
            if (avdl <= 0) return 0.0;

            var tf = posting.TermFrequency;
            var dl = docs.GetLength(posting.DocumentId, posting.Field);

            double bm25 = BM25.ComputeScore(tf, df, n, dl, avdl, options.BM25K, options.BM25B);
            
            if (double.IsNaN(bm25) || double.IsInfinity(bm25)) return 0.0;
            
            return bm25;
        }
    }
}
