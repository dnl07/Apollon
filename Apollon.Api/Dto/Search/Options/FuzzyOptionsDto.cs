namespace Apollon.Api.Dto.Search {
    public class FuzzyOptionsDto {
        public int MaxEditDistance { get; set; } = 0;
        public int MaxPrefixEditDistance { get; set; } = 0;
        public int EditDistanceLimit { get; set; } =  0;
    }
}