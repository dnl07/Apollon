using Apollon.Core.Analysis;
using Apollon.Core.Documents;
using Apollon.Core.Options;

namespace Apollon.Core.Fuzzy {
    public class QueryExpander {
        public List<(string token, double boost)> Expand(
            string request,
            FuzzyMatcher fuzzyMatcher,
            TokenRegistry tokenRegistry,
            QueryOptions options) {
            var expanded = new List<(string term, double boost)>();

            foreach (string term in Tokenizer.Tokenize(request)) {
                expanded.Add((term, 1.0));

                foreach (var fuzzy in fuzzyMatcher
                    .Match(term, tokenRegistry, options)
                    .OrderBy(f => f.EditDistance)
                    .Take(options.EditDistanceLimit)) {
                    double boost = term == fuzzy.Token ? 5 : Math.Exp(-fuzzy.EditDistance);
                    expanded.Add((fuzzy.Token, boost));
                }
            }
            return expanded;
        }
    }
}
