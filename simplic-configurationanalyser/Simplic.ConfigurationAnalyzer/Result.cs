using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ConfigurationAnalyzer
{
    /// <summary>
    /// Single configuration result
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Gets or sets the configuration name, like GRID_...
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the configuration type, like grid, ...
        /// </summary>
        public string ConfigurationType { get; set; }

        /// <summary>
        /// Gets or sets the result type (error, ...)
        /// </summary>
        public ResultType ResultType { get; set; }

        /// <summary>
        /// Gets or sets the message result
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the analyzer name
        /// </summary>
        public string AnalyzerName { get; set; }
    }
}
