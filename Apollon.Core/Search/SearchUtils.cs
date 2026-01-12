using Apollon.Core.Documents;
using Apollon.Core.Indexing;
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

        /// <summary>
        /// Merges two already sorted lists of postings.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static List<Posting> MergePostingsLists(List<Posting> a, List<Posting> b) {
            if (a.Count == 0) return [..b];
            if (b.Count == 0) return [..a];

            List<Posting> result = [];

            int i = 0;
            int j = 0;

            while (i < a.Count && j < b.Count) {
                if (a[i].DocumentId == b[j].DocumentId) {
                    a[i].Score += b[j].Score;
                    result.Add(a[i]);
                    i++;
                    j++;
                } else if (a[i].DocumentId.CompareTo(b[j].DocumentId) < 0) {
                    result.Add(a[i]);
                    i++;
                } else {
                    result.Add(b[j]);
                    j++;
                }
            }

            while (i < a.Count) {
                result.Add(a[i]);
                i++;
            }

            while (j < b.Count) {
                result.Add(b[j]);
                j++;
            }

            return result;
        }
    }
}
