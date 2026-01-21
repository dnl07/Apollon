using Apollon.Models.Indexing;

namespace Apollon.Models.Scoring {
    public class ScoreContribution {
        public Field Field { get; set; }
        public double BM25 { get; set; }
        public double Boost { get; set; }
        public double FieldWeight { get; set; }
        public double Final { get; set; }      
    }
}