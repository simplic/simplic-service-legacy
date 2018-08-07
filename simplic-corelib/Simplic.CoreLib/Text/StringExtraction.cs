using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Simplic.Text
{
    /// <summary>
    /// Contains methods to extract data from strings
    /// </summary>
    public static class StringExtraction
    {
        /// <summary>
        /// Searches from date values in a string
        /// </summary>
        /// <param name="input">Input string that will be used for searching</param>
        /// <returns>Returns a <see cref="DateTime"/> instance of the text contains a date time in the format (dd.MM.yyyy HH:mm) HH:mm is optional</returns>
        public static DateTime? ExtractDateTime(string input)
        {
            var regex_date = new Regex(@"[0-9]{1,2}\.[0-9]{1,2}\.[0-9]{4}");
            var regex_dateTime = new Regex(@"[0-9]{2}\.[0-9]{2}\.[0-9]{4}\s[0-9]{2}\:[0-9]{2}");

            foreach (Match m in regex_dateTime.Matches(input))
            {
                DateTime dt;
                if (DateTime.TryParseExact(m.Value, "dd.MM.yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out dt))
                {
                    return dt;
                }
            }

            foreach (Match m in regex_date.Matches(input))
            {
                DateTime dt;
                if (DateTime.TryParseExact(m.Value, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out dt))
                {
                    return dt;
                }
            }

            return null;
        }

        /// <summary>
        /// Find value within text block
        /// </summary>
        /// <param name="textBlock">Text block as string</param>
        /// <param name="keys">List of keys to search for</param>
        /// <param name="regex">Regex which matches the value</param>
        /// <param name="charsToRemove">Chars to remove from a key while comparing</param>
        /// <returns>Result instance, which fits the most</returns>
        public static ExtractionResult FindInLine(string textBlock, IList<ExtractionKey> keys, string regex, string charsToRemove = ".:;")
        {
            var whiteList = new List<ExtractionValue>();

            if (!string.IsNullOrWhiteSpace(regex))
            {
                MatchCollection matchList = Regex.Matches(textBlock, regex);
                whiteList = matchList.Cast<Match>().Select(match => match.Value).Select(x => new ExtractionValue { Value = x }).ToList();
            }

            return FindInLine(textBlock, keys, whiteList, charsToRemove);
        }

        /// <summary>
        /// Find value within text block
        /// </summary>
        /// <param name="textBlock">Text block as string</param>
        /// <param name="keys">List of keys to search for</param>
        /// <param name="valueWhiteList">List of allowed values</param>
        /// <param name="charsToRemove">Chars to remove from a key while comparing</param>
        /// <returns>Result instance, which fits the most</returns>
        public static ExtractionResult FindInLine(string textBlock, IList<ExtractionKey> keys, IList<ExtractionValue> valueWhiteList, string charsToRemove = ".:;")
        {
            var values = new List<ExtractionResult>();

            // Clean keys
            foreach (var charToRemove in charsToRemove)
            {
                foreach (var key in keys)
                {
                    key.Key = key.Key.Replace(charToRemove.ToString(), "");
                }
            }

            var lines = textBlock.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                // Split words
                var words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Go though all words, SKIP the last one
                for (int i = 0; i < words.Length - 1; i++)
                {
                    var word = words[i];
                    var cleanedWord = word;

                    foreach (var charToRemove in charsToRemove)
                    {
                        cleanedWord = cleanedWord.Replace(charToRemove.ToString(), "");
                    }

                    foreach (var key in keys)
                    {
                        // Compare word an key
                        var similarity = LevenshteinDistance.Compute(key.Key, cleanedWord);
                        if (similarity <= 3)
                        {
                            var valueString = words[i + 1];

                            var value = new ExtractionResult
                            {
                                Similarity = similarity,
                                Key = key,
                                Value = valueWhiteList.FirstOrDefault(x => x.Value == words[i + 1]),
                                OriginalKey = word,
                                CleanedKey = cleanedWord
                            };

                            value.ValueMatched = value.Value != null;

                            values.Add(value);
                        }
                    }
                }
            }

            // Return most similar value
            return values.OrderByDescending(x => x.ValueMatched).ThenBy(x => x.Similarity).FirstOrDefault();
        }

        /// <summary>
        /// Split line into a list of strings
        /// </summary>
        /// <param name="line">Line to split</param>
        /// <param name="splitChar">Split chars</param>
        /// <param name="minCharBeforeSplit">Minimum chars before splitting</param>
        /// <param name="minPartLength">Minimum length</param>
        /// <param name="cleanupChars">Chars to cleanup when length is 0</param>
        /// <returns></returns>
        public static IList<string> SplitLine(string line, char splitChar, int minCharBeforeSplit = 1, int minPartLength = 0, string cleanupChars = "1234567890abcdefghijklmnopqrstuvwxyz")
        {
            var parts = new List<string>();
            int splitCharCount = 0;
            var currentPart = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                var current = line[i];
                if (splitChar == current)
                {
                    splitCharCount++;
                }

                if (splitCharCount >= minCharBeforeSplit)
                {
                    splitCharCount = 0;

                    if (currentPart.Length > 0)
                    {
                        var part = currentPart.ToString().Trim(splitChar);

                        if (!string.IsNullOrWhiteSpace(cleanupChars) && part.Length == 1)
                        {
                            if (!cleanupChars.Contains(part[0]))
                                part = "";
                        }

                        if (!string.IsNullOrWhiteSpace(part) && (minPartLength == 0 || part.Length >= minPartLength))
                        {
                            parts.Add(part);
                        }
                    }

                    currentPart = new StringBuilder();
                }
                else
                {
                    if (splitChar != current)
                        splitCharCount = 0;
                    currentPart.Append(current);
                }
            }

            if (currentPart.Length > 0)
            {
                var part = currentPart.ToString().Trim(splitChar);

                if (!string.IsNullOrWhiteSpace(cleanupChars) && part.Length == 1)
                {
                    if (!cleanupChars.Contains(part[0]))
                        part = "";
                }

                if (!string.IsNullOrWhiteSpace(part) && (minPartLength == 0 || part.Length >= minPartLength))
                    parts.Add(part);
            }

            return parts;
        }

        /// <summary>
        /// Extract a number from string and remove thousand-separator
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="separator">Separator char</param>
        /// <returns>Extracted double or null. Throws an exception if casting failed</returns>
        public static double CastAsNumber(string input, char separator)
        {
            var value = 0d;

            if (!string.IsNullOrWhiteSpace(input))
            {
                var validChars = "1234567890";
                var minus = '-';
                var cleanValue = new StringBuilder();
                bool separatorReplaced = false;

                for (int i = input.Length - 1; i >= 0; i--)
                {
                    var current = input[i];
                    if (validChars.Contains(current) || (i == 0 && current == minus))
                    {
                        cleanValue.Insert(0, current);
                    }
                    else if (i > 0)
                    {
                        if ((current == '.' || current == ',') && !separatorReplaced)
                        {
                            cleanValue.Insert(0, separator);
                            separatorReplaced = true;
                        }
                    }
                }

                value = double.Parse(cleanValue.ToString());
            }

            return value;
        }
    }
}
