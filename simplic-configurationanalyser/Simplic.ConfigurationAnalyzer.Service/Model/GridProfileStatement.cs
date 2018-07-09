using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ConfigurationAnalyzer.Service
{
    /// <summary>
    /// Helper result
    /// </summary>
    internal class GridProfileStatement
    {
        /// <summary>
        /// Gets or sets the grid name
        /// </summary>
        public string GridName { get; set; }

        /// <summary>
        /// Gets or sets the statement
        /// </summary>
        public string Statement { get; set; }

        /// <summary>
        /// Gets or sets the profile name
        /// </summary>
        public string ProfileName { get; set; }
    }
}
