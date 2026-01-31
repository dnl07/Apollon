namespace Apollon.Api.Dto.Search {
    public class FuzzyOptionsDto {
        public int MaxEditDistance { get; set; } = 2;
        public int MaxPrefixEditDistance { get; set; } = 1;
        public int EditDistanceLimit { get; set; } =  2;
    }
}