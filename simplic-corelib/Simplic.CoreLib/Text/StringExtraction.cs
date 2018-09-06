using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

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

        private class FindNextLineMatch
        {
            public int LineIndex { get; set; }
            public int WordIndex { get; set; }
            public string OriginalKey { get; set; }
            public ExtractionKey Key { get; set; }
            public int Distance { get; set; }
        }

        /// <summary>
        /// Find value within text block
        /// </summary>
        /// <param name="textBlock">Text block as string</param>
        /// <param name="keys">List of keys to search for</param>
        /// <param name="regex">Regex which matches the value</param>
        /// <param name="charsToRemove">Chars to remove from a key while comparing</param>
        /// <returns>Result instance, which fits the most</returns>
        public static ExtractionResult FindInNextLine(string textBlock, IList<ExtractionKey> keys, string regex, Func<string, bool> validateValue = null, string charsToRemove = ".:;", int minSplitChars = 3)
        {
            var whiteList = new List<ExtractionValue>();

            if (!string.IsNullOrWhiteSpace(regex))
            {
                var lines = textBlock.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    MatchCollection matchList = Regex.Matches(textBlock, regex);
                    whiteList.AddRange(matchList.Cast<Match>().Select(match => match.Value).Select(x => new ExtractionValue { Value = x }));
                }
            }

            return FindInNextLine(textBlock, keys, whiteList, validateValue, charsToRemove, minSplitChars);
        }

        /// <summary>
        /// Find value within text block and retrieve it from the next line
        /// </summary>
        /// <param name="textBlock">Text block as string</param>
        /// <param name="keys">List of keys to search for</param>
        /// <param name="valueWhiteList">List of allowed values</param>
        /// <param name="charsToRemove">Chars to remove from a key while comparing</param>
        /// <returns>Result instance, which fits the most</returns>
        public static ExtractionResult FindInNextLine(string textBlock, IList<ExtractionKey> keys, IList<ExtractionValue> valueWhiteList, Func<string, bool> validateValue = null, string charsToRemove = ".:;", int minSplitChars = 3, bool forceWhiteList = false)
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
            var splittedReversedLines = new List<IList<string>>();
            foreach (var line in lines)
            {
                splittedReversedLines.Add(SplitLine(line, ' ', minSplitChars, 2).Reverse().ToList());
            }

            var lineIndex = 0;
            ExtractionKey matchedKey = null;
            int distance = 0;
            var matches = new List<FindNextLineMatch>();

            foreach (var line in splittedReversedLines)
            {
                int wordIndex = 0;

                foreach (var word in line)
                {
                    var cleanedWord = word;
                    foreach (var charToRemove in charsToRemove)
                    {
                        cleanedWord = cleanedWord.Replace(charToRemove.ToString(), "");
                    }

                    foreach (var key in keys)
                    {
                        distance = LevenshteinDistance.Compute(key.Key, cleanedWord);
                        if (distance <= 3)
                        {
                            matchedKey = key;
                            break;
                        }
                    }

                    if (matchedKey != null)
                    {
                        matches.Add(new FindNextLineMatch
                        {
                            Key = matchedKey,
                            LineIndex = lineIndex,
                            WordIndex = wordIndex,
                            OriginalKey = cleanedWord,
                            Distance = distance
                        });

                        matchedKey = null;
                        break;
                    }

                    wordIndex++;
                }

                lineIndex++;
            }

            foreach (var match in matches)
            {
                if (splittedReversedLines.Count > match.LineIndex + 1)
                {
                    var line = splittedReversedLines[match.LineIndex + 1];
                    if (line.Count > match.WordIndex)
                    {
                        var valueString = line[match.WordIndex];

                        if (validateValue == null || validateValue(valueString))
                        {
                            if (forceWhiteList && !valueWhiteList.Any(x => x.Value == valueString))
                            {
                                // Skip not validated match
                            }
                            else
                            {
                                // Add as validated value
                                if (validateValue != null)
                                    valueWhiteList.Add(new ExtractionValue { Value = valueString });

                                var value = new ExtractionResult
                                {
                                    KeyDistance = match.Distance,
                                    Key = matchedKey,
                                    Value = valueWhiteList.FirstOrDefault(x => x.Value == valueString),
                                    OriginalValue = valueString,
                                    CleanedKey = match.OriginalKey
                                };

                                value.ValueMatched = value.Value != null;

                                values.Add(value);
                            }
                        }
                    }
                }
            }

            // Return most similar value
            return values.OrderByDescending(x => x.ValueMatched).ThenBy(x => x.KeyDistance).FirstOrDefault();
        }

        /// <summary>
        /// Find value within text block
        /// </summary>
        /// <param name="textBlock">Text block as string</param>
        /// <param name="keys">List of keys to search for</param>
        /// <param name="regex">Regex which matches the value</param>
        /// <param name="charsToRemove">Chars to remove from a key while comparing</param>
        /// <returns>Result instance, which fits the most</returns>
        public static ExtractionResult FindInLine(string textBlock, IList<ExtractionKey> keys, string regex, Func<string, bool> validateValue = null, string charsToRemove = ".:;", int minResultLenght = 3)
        {
            var whiteList = new List<ExtractionValue>();

            if (!string.IsNullOrWhiteSpace(regex))
            {
                var lines = textBlock.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    MatchCollection matchList = Regex.Matches(textBlock, regex);
                    whiteList.AddRange(matchList.Cast<Match>().Select(match => match.Value).Select(x => new ExtractionValue { Value = x }));
                }
            }

            return FindInLine(textBlock, keys, whiteList, validateValue, charsToRemove, minResultLenght: minResultLenght);
        }

        /// <summary>
        /// Find value within text block
        /// </summary>
        /// <param name="textBlock">Text block as string</param>
        /// <param name="keys">List of keys to search for</param>
        /// <param name="valueWhiteList">List of allowed values</param>
        /// <param name="charsToRemove">Chars to remove from a key while comparing</param>
        /// <returns>Result instance, which fits the most</returns>
        public static ExtractionResult FindInLine(string textBlock, IList<ExtractionKey> keys, IList<ExtractionValue> valueWhiteList, Func<string, bool> validateValue = null, string charsToRemove = ".:;", bool forceWhiteList = false, int minResultLenght = 3)
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
                    var keyMatched = false;
                    var similarity = 0;
                    ExtractionKey matchedKey = null;

                    foreach (var charToRemove in charsToRemove)
                    {
                        cleanedWord = cleanedWord.Replace(charToRemove.ToString(), "");
                    }

                    foreach (var key in keys)
                    {
                        // Compare word an key
                        similarity = LevenshteinDistance.Compute(key.Key, cleanedWord);
                        if (similarity <= 3)
                        {
                            keyMatched = true;
                            matchedKey = key;
                            break;
                        }
                    }

                    if (keyMatched)
                    {
                        // Continue line loop
                        for (i = i + 1; i < words.Length; i++)
                        {
                            var valueString = words[i];

                            if (valueString.Length < minResultLenght)
                                continue;

                            // Check value-string is not similar to a key
                            if (valueString != null && valueString.Length >= 3)
                            {
                                foreach (var key in keys)
                                {
                                    if (key.Key.Length <= 3)
                                        continue;

                                    // Compare value and keys
                                    var keyValueDistance = LevenshteinDistance.Compute(key.Key, valueString);
                                    if (keyValueDistance <= 3)
                                        continue;
                                }
                            }

                            // Validate values
                            if (validateValue == null || validateValue(valueString))
                            {
                                if (forceWhiteList && !valueWhiteList.Any(x => x.Value == valueString))
                                    continue;

                                // Add as validated value
                                if (validateValue != null)
                                    valueWhiteList.Add(new ExtractionValue { Value = valueString });

                                var value = new ExtractionResult
                                {
                                    KeyDistance = similarity,
                                    Key = matchedKey,
                                    Value = valueWhiteList.FirstOrDefault(x => x.Value == valueString),
                                    OriginalValue = valueString,
                                    CleanedKey = cleanedWord
                                };

                                value.ValueMatched = value.Value != null;

                                values.Add(value);

                                // Break loop, because a word was found
                                i = int.MaxValue - 2;
                                break;
                            }
                        }

                        // Reset matches
                        keyMatched = false;
                        similarity = 0;
                        matchedKey = null;
                    }
                }
            }

            // Return most similar value
            return values.OrderByDescending(x => x.ValueMatched).ThenBy(x => x.KeyDistance).FirstOrDefault();
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
        /// <param name="minDecimalNumbers">Minimum decimal numbers</param>
        /// <param name="maxAlphanumericChars"></param>
        /// <returns>Extracted double or null. Throws an exception if casting failed</returns>
        public static double CastAsNumber(string input, int minDecimalNumbers = 2, int maxAlphanumericChars = 2)
        {
            return CastAsNumber(input, new[] { (char)8218, '.', ',' }, minDecimalNumbers, maxAlphanumericChars);
        }

        /// <summary>
        /// Extract a number from string and remove thousand-separator
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="separators"></param>
        /// <param name="minDecimalNumbers">Minimum decimal numbers</param>
        /// <param name="maxAlphanumericChars"></param>
        /// <returns>Extracted double or null. Throws an exception if casting failed</returns>
        public static double CastAsNumber(string input, IList<char> separators, int minDecimalNumbers = 2, int maxAlphanumericChars = 2)
        {
            var value = 0d;

            if (!string.IsNullOrWhiteSpace(input))
            {
                var validChars = "1234567890";
                var minus = '-';
                var cleanValue = new StringBuilder();
                bool separatorReplaced = false;
                int currentAlphanumericChars = 0;

                for (int i = input.Length - 1; i >= 0; i--)
                {
                    var current = input[i];
                    if (validChars.Contains(current) || (i == 0 && current == minus))
                    {
                        cleanValue.Insert(0, current);
                    }
                    else if (i > 0)
                    {
                        if ((separators.Contains(current)) && !separatorReplaced && cleanValue.Length >= minDecimalNumbers)
                        {
                            cleanValue.Insert(0, '.');
                            separatorReplaced = true;
                        }
                        else
                        {
                            if (currentAlphanumericChars >= maxAlphanumericChars)
                                throw new InvalidCastException($"Could not cast to number {input}. To many alphanumeric chars.");

                            currentAlphanumericChars++;
                        }
                    }
                }

                value = double.Parse(cleanValue.ToString(), CultureInfo.InvariantCulture);
            }

            return value;
        }
    }
}
