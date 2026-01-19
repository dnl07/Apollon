using Apollon.Api.Dto.Documents;
using Apollon.Core.Documents;

namespace Apollon.Api.Mappers.Document {
    public static class DocumentMapper {
        public static SearchDocument ToEngineModel(this DocumentRequestDto request) {
            return new SearchDocument {
                Title = request.Title,
                Description = request.Description,
                Tags = request.Tags
            };
        }
        public static DocumentDto ToDto(this SearchDocument doc) {
            return new DocumentDto {
                Id = doc.Id,
                Title = doc.Title
            };
        }
    }
}