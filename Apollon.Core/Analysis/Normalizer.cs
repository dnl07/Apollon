using System.Globalization;
using System.Text;

namespace Apollon.Core.Analysis {
    public static class Normalizer {
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
                '"', '\'', '�', '�', '�'
            );

            if (!token.Any(char.IsLetterOrDigit)) return string.Empty;

            return token;
        }

        /// <summary>
        /// Removes diacritics from a given string.
        /// </summary>
        private static string RemoveDiacritics(string token) {
            var normalizedString = token.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++) {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark) {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }
    }
}