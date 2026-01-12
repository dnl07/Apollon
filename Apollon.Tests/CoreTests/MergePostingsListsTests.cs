using Apollon.Core.Indexing;
using Apollon.Core.Search;
using Xunit;

namespace Apollon.Tests.CoreTests {
    public class MergePostingsListsTests {
        private static Posting P(Guid id, double score = 0)
            => new Posting(id, 1) { Score = score };

        [Fact]
        public void Merge_TwoEmptyLists_ReturnsEmpty() {
            var a = new List<Posting>();
            var b = new List<Posting>();

            var result = SearchUtils.MergePostingsLists(a, b);

            Assert.Empty(result);
        }

        [Fact]
        public void Merge_OneEmptyOneNonEmpty_ReturnsOther() {
            var id = Guid.NewGuid();

            var a = new List<Posting>();
            var b = new List<Posting> { P(id, 1.5) };

            var result = SearchUtils.MergePostingsLists(a, b);

            Assert.Single(result);
            Assert.Equal(id, result[0].DocumentId);
            Assert.Equal(1.5, result[0].Score);
        }

        [Fact]
        public void Merge_NoOverlappingDocuments_ConcatenatesSorted() {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var a = new List<Posting> { P(id1, 1) };
            var b = new List<Posting> { P(id2, 2) };

            var result = SearchUtils.MergePostingsLists(a, b);

            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.DocumentId == id1);
            Assert.Contains(result, p => p.DocumentId == id2);
        }
        [Fact]
        public void Merge_OverlappingDocuments_SumsScores() {
            var id = Guid.NewGuid();

            var a = new List<Posting> { P(id, 1.0) };
            var b = new List<Posting> { P(id, 2.5) };

            var result = SearchUtils.MergePostingsLists(a, b);

            Assert.Single(result);
            Assert.Equal(id, result[0].DocumentId);
            Assert.Equal(3.5, result[0].Score, precision: 5);
        }

        [Fact]
        public void Merge_MultipleOverlapsAndUniques_WorksCorrectly() {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var id3 = Guid.NewGuid();

            var a = new List<Posting> {
                P(id1, 1),
                P(id2, 2)
            };

            var b = new List<Posting> {
                P(id2, 3),
                P(id3, 4)
            };

            var result = SearchUtils.MergePostingsLists(a, b);

            Assert.Equal(3, result.Count);

            Assert.Equal(1, result.Single(p => p.DocumentId == id1).Score);
            Assert.Equal(5, result.Single(p => p.DocumentId == id2).Score);
            Assert.Equal(4, result.Single(p => p.DocumentId == id3).Score);
        }
    }
}
