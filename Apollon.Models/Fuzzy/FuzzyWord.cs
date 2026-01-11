namespace Apollon.Models.Fuzzy {
    public class FuzzyWord {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Word { get; set; } = "";
    }
}
