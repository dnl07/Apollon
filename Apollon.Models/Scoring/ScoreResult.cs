namespace Apollon.Models.Scoring {
    public class ScoreResult {
    public double FinalScore { get; set; }
    public Dictionary<string, List<ScoreContribution>> Contributions { get; set; } = new();
}
}