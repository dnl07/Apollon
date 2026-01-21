namespace Apollon.Api.Dto.Search {
    public class BoostOptionsDto {
        public float Title { get; set; } = 0;
        public float Description { get; set; } = 0;
        public float Tags { get; set; } = 0;
    }
}