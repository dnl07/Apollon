namespace Apollon.Api.Dto.Search {
    public class SearchExplainDto {
        public BM25Dto BM25 = new BM25Dto();
        public double FinalScore = 0;
    }
}