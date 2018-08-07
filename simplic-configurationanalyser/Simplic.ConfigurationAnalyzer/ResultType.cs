using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ConfigurationAnalyzer
{
    /// <summary>
    /// Represents the result type
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// Warning, should be solved
        /// </summary>
        Warning,

        /// <summary>
        /// Error, must be solved
        /// </summary>
        Error
    }
}
