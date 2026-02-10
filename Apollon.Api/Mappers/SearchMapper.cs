using Apollon.Api.Dto.Search;
using Apollon.Models.Search;

namespace Apollon.Api.Mappers.Search {
    public static class SearchMapper {
        public static SearchResponseDto ToDto(this SearchResult result) {
            List<SearchHitDto> hits = new();

            foreach (var hit in result.Hits) {
                var hitDto = new SearchHitDto {
                    Id = hit.Document.Id,
                    Fields = new SearchFieldDto {
                        Title = hit.Document.Title,
                        Description = hit.Document.Description,
                        Tags = hit.Document.Tags
                    },
                };
                if (hit.Explain is not null) {
                    hitDto.Explain = new SearchExplainDto {
                        FinalScore = hit.Explain.FinalScore,
                        Contributions = hit.Explain.Contributions
                    };
                }
                hits.Add(hitDto);
            }

            return new SearchResponseDto {
                Query = result.Query,
                Total = result.Hits.Count,
                MatchedTokens = result.MatchedTokens,
                Hits = hits.ToArray()
            };
        }
    }
}