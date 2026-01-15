using Apollon.Core.Analysis;
using Apollon.Core.Documents;
using Apollon.Core.Fuzzy;
using Apollon.Core.Indexing;
using Apollon.Core.Options;
using Apollon.Core.Ranking;

namespace Apollon.Core.Search {
    public static class SearchUtils {
        /// <summary>
        /// Calculates the BM25-Score for every posting in a list given all documents.
        /// </summary>
        public static double ComputeBM25Score(Posting posting, int df, DocumentStore docs) {
            int n = docs.Count;
            double avdl = docs.AverageDocumentLength;

            var tf = posting.TermFrequency;
            var dl = docs.GetLength(posting.DocumentId);

            return BM25.ComputeScore(tf, df, n, dl, avdl, SearchConstants.K, SearchConstants.B);
        }

        public static List<(string token, double boost)> FuzzySearch(
            string request, 
            FuzzyMatcher fuzzyMatcher,
            TokenRegistry tokenRegistry,
            QueryOptions options) {
            var expanded = new List<(string term, double boost)>();

            foreach (string term in Tokenizer.Tokenize(request)) {
                expanded.Add((term, 1.0));

                foreach (var fuzzy in fuzzyMatcher.Match(term, tokenRegistry, options)) {
                    double boost = Math.Exp(-fuzzy.EditDistance);
                    expanded.Add((fuzzy.Token, boost));
                }
            }
            return expanded;
        }

        public static Dictionary<Guid, double> CreateScores(
            List<(string token, double boost)> expanded, 
            InvertedIndex _invertedIndex,
            DocumentStore docs) {

            var scores = new Dictionary<Guid, double>();

            foreach ((string term, double boost) in expanded) {
                var postings = _invertedIndex.GetSortedPostings(term);
                int df = postings.Count;

                foreach (var posting in postings) {
                    double bm25 = ComputeBM25Score(posting, df, docs);
                    
                    double finalScore = bm25 * boost;

                    if (scores.TryGetValue(posting.DocumentId, out var currentScore)) {
                        scores[posting.DocumentId] = currentScore + finalScore;
                    } else {
                        scores[posting.DocumentId] = finalScore;
                    }
                }
            }
            return scores;
        }
    }
}