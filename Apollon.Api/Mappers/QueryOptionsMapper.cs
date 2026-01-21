using Apollon.Api.Dto.Search;
using Apollon.Core.Options;

namespace Apollon.Api.Mappers.Options {
    public static class QueryOptionsMapper {
        public static QueryOptions ToEngineModel(this QueryOptionsDto options) {
            return new QueryOptions {
                MaxDocs = options.Limit,
                MaxEditDistance = options.Fuzzy.EditDistanceLimit,
                EditDistanceLimit = options.Fuzzy.EditDistanceLimit,
                MaxPrefixEditDistance = options.Fuzzy.MaxPrefixEditDistance,
                BM25K = options.Score.K,
                BM25B = options.Score.B,
                TitleWeight = options.Score.boost.Title,
                DescriptionWeight = options.Score.boost.Description,
                TagWeight = options.Score.boost.Tags,
            };
        }
    }
}