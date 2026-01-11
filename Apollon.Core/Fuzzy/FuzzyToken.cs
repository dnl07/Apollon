namespace Apollon.Core.Fuzzy {
    public readonly record struct FuzzyToken (
        string Token,
        int EditDistance
    );
}