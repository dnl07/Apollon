using Apollon.Core.Documents;
using Apollon.Core.Indexing;
using Apollon.Models.Indexing;
using Xunit;

namespace Apollon.Tests.CoreTests {
    public class InvertedIndexTests {
        [Fact]
        public void Adding_Tokens_ReturnsExpectedIndex() {
            var index = new InvertedIndex();

            var doc1 = new SearchDocument {
                Id = Guid.NewGuid(),
                Title = "Title example",
                Description = "Description",
                Tags = ["string", "document", "example"]
            };
            var doc2 = new SearchDocument {
                Id = Guid.NewGuid(),
                Title = "Title",
                Description = "",
                Tags = ["string", "search"]
            };

            doc1.Tokenize();
            doc2.Tokenize();

            index.AddDocument(doc1);
            index.AddDocument(doc2);

            /*Assert.Collection(titleTokens, 
                p => {
                    Assert.Equal(doc1.Id, p.DocumentId);
                    Assert.Equal(Field.Title, p.Field);
                    Assert.Equal(1, p.TermFrequency);
                }
            );*/
        }
    }
}