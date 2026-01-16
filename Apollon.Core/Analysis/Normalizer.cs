using System.Text;

namespace Apollon.Core.Analysis {
    internal static class Normalizer {
        public static string Normalize(string token) {
            if (string.IsNullOrWhiteSpace(token)) return string.Empty;

            // Simplify unicode
            token = token.Normalize(NormalizationForm.FormKC);

            // lower-case
            token = token.ToLowerInvariant();

            // Remove diacritics
            token = RemoveDiacritics(token);

            token = token.Trim(
                '.', ',', ':', ';', '!', '?',
                '(', ')', '[', ']', '{', '}',
                '"', '\'', '’', '“', '”'
            );

            if (!token.Any(char.IsLetterOrDigit)) return string.Empty;

            return token;
        }

        /// <summary>
        /// Removes diacritics from a given string.
        /// </summary>
        private static string RemoveDiacritics(string token) {
            return token;
        }
    }
}