using Apollon.Models.Scoring;

namespace Apollon.Api.Dto.Search {
    public class SearchExplainDto {
        public double FinalScore { get; set; }
        public Dictionary<string, List<ScoreContribution>> Contributions { get; set; } = new();
    }
}