namespace Apollon.Api.Dto.Search {
    public class ScoreOptionsDto {
        public double K = 0;
        public double B = 0;
        public BoostOptionsDto boost = new BoostOptionsDto();
    }
}