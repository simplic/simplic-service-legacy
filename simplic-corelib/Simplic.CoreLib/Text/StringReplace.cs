using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Text
{
    /// <summary>
    /// String replace helper
    /// </summary>
    public static class StringReplace
    {
        /// <summary>
        /// Replace first word in text
        /// </summary>
        /// <param name="text">Text to search in</param>
        /// <param name="search">Text to search</param>
        /// <param name="replace">Replace string</param>
        /// <returns>Input text with first replaced char if exists</returns>
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int position = text.IndexOf(search);
            if (position < 0)
                return text;

            return text.Substring(0, position) + replace + text.Substring(position + search.Length);
        }
    }
}
