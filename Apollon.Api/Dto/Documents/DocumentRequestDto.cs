namespace Apollon.Api.Dto.Documents {
    public class DocumentRequestDto {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string[] Tags { get; set; } = [];
    }
}