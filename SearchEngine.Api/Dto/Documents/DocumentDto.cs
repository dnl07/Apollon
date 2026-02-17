namespace SearchEngine.Api.Dto.Documents {
    public class DocumentDto {
        public Guid Id  { get; set; } = Guid.Empty;
        public string Title  { get; set; } = "";
    }
}