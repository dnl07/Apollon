using Apollon.Api.Dto.Engine;
using Apollon.Models.Search;

namespace Apollon.Api.Mappers.Status {
    public static class StatusMapper {
        public static StatusDto ToDto(this SearchStatus status) {
            return new StatusDto {
                IsRunning = status.IsRunning,
                StartetAt = status.StartetAt,
                TotalDocuments = status.TotalDocuments,
                TotalTokens = status.TotalTokens,
                TotalNGrams = status.TotalNGrams
            };
        }
    }
}