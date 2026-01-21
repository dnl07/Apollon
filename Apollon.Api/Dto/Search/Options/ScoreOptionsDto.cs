namespace Apollon.Api.Dto.Search {
    public class ScoreOptionsDto {
        public double K { get; set; } = 0;
        public double B { get; set; } = 0;
        public BoostOptionsDto boost { get; set; } = new BoostOptionsDto();
    }
}