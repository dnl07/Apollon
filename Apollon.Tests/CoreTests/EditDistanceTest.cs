using Apollon.Core.Fuzzy;
using Xunit;

namespace Apollon.Tests.CoreTests {
    public class EditDistanceTest {
        [Fact]
        public void Test() {
            Assert.Equal(0, EditDistance.Calculate("", ""));
            Assert.Equal(0, EditDistance.Calculate("edit", "edit"));
            Assert.Equal(4, EditDistance.Calculate("edit", ""));
            Assert.Equal(8, EditDistance.Calculate("", "distance"));
            Assert.Equal(1, EditDistance.Calculate("distance", "distanc"));    // insertion
            Assert.Equal(1, EditDistance.Calculate("distanc", "distance"));    // deletion
            Assert.Equal(1, EditDistance.Calculate("distance", "distancc"));    // replacement
            Assert.Equal(2, EditDistance.Calculate("edit", "ed"));
            Assert.Equal(6, EditDistance.Calculate("edit", "distance"));
        }
    }
}
