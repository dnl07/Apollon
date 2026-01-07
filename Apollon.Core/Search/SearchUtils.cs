using Apollon.Core.Documents;
using Apollon.Core.Indexing;
using Apollon.Core.Ranking;

namespace Apollon.Core.Search {
    internal static class SearchUtils {
        public static void ComputeBM25Scores(List<Posting> postings, DocumentStore docs) {
            int n = docs.Count;
            double avdl = docs.AverageDocumentLength;

            foreach (var posting in postings) {
                var tf = posting.TermFrequency;
                var df = postings.Count;
                var dl = docs.GetLength(posting.DocumentId);

                posting.BM25Score = BM25.ComputeScore(tf, df, n, dl, avdl, SearchConstants.K, SearchConstants.B);
            }
        }

        public static List<Posting> MergePostingsLists(List<Posting> a, List<Posting> b) {
            List<Posting> result = [];

            int i = 0;
            int j = 0;

            while (i < a.Count && j < b.Count) {
                if (a[i].DocumentId == b[j].DocumentId) {
                    a[i].BM25Score += b[j].BM25Score;
                    result.Add(a[i]);
                    i++;
                } else if (a[i].DocumentId < b[j].DocumentId) {
                    result.Add(a[i]);
                    i++;
                } else {
                    result.Add(b[i]);
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
