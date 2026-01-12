using Apollon.Core.Indexing;
using Xunit;

namespace Apollon.Tests.CoreTests {
    public class SearchUtilsTests {
        [Fact]
        public void MergePostingsTest() {
            var posting1 = new Posting(Guid.NewGuid(), 5);
            posting1.Score = 5;
            var posting2 = new Posting(Guid.NewGuid(), 10);
            var posting3 = new Posting(Guid.NewGuid(), 8);
            var posting4 = new Posting(Guid.NewGuid(), 2);

        }
    }
}
