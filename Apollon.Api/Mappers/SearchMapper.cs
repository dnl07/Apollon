using Apollon.Api.Dto.Documents;
using Apollon.Api.Dto.Search;
using Apollon.Core.Documents;
using Apollon.Models.Search;

namespace Apollon.Api.Mappers.Search {
    public static class SearchMapper {
        public static SearchDocument ToEngineModel(this DocumentRequestDto request) {
            return new SearchDocument {
                Title = request.Title,
                Description = request.Description,
                Tags = request.Tags
            };
        }
        public static SearchResponseDto ToDto(this SearchResult result) {
            List<SearchHitDto> hits = new();

            foreach (var doc in result.Documents) {
                hits.Add(new SearchHitDto {
                    Id = doc.Id,
                    Fields = new SearchFieldDto {
                        Title = doc.Title,
                        Description = doc.Description,
                        Tags = doc.Tags
                    }
                });
            }

            return new SearchResponseDto {
                Query = result.Query,
                Total = result.Documents.Count,
                Hits = hits.ToArray()
            };
        }
    }
}