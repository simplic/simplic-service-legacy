using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Text
{
    /// <summary>
    /// Extraction result
    /// </summary>
    public class ExtractionResult
    {
        /// <summary>
        /// Gets sets the similarity. Lower = more equal
        /// </summary>
        public int KeyDistance { get; set; }

        /// <summary>
        /// Gets or sets the extraction key
        /// </summary>
        public ExtractionKey Key { get; set; }

        /// <summary>
        /// Gets or sets the original key word
        /// </summary>
        public string OriginalValue { get; set; }

        /// <summary>
        /// Gets or sets the raw value
        /// </summary>
        public ExtractionValue Value { get; set; }

        /// <summary>
        /// Gets or sets whether the values matches the white-list
        /// </summary>
        public bool ValueMatched { get; set; }

        /// <summary>
        /// Gets or sets the cleaned key
        /// </summary>
        public string CleanedKey { get; set; }
    }

    /// <summary>
    /// Extraction value
    /// </summary>
    public class ExtractionValue
    {
        /// <summary>
        /// Gets or sets the value to compate
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the id of the key
        /// </summary>
        public object Id { get; set; }
    }

    /// <summary>
    /// Extraction key
    /// </summary>
    public class ExtractionKey
    {
        /// <summary>
        /// Gets or sets the key to compare
        /// </summary>
        public string Key { get; set; }
    }
}
