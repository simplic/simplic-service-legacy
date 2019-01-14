using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ConfigurationAnalyzer
{
    /// <summary>
    /// Configuration analyzer service
    /// </summary>
    public interface IConfigurationAnalyzerService
    {
        /// <summary>
        /// Analyze the database
        /// </summary>
        /// <returns>List of results</returns>
        IList<Result> Analyze();

        /// <summary>
        /// Convert a result to text
        /// </summary>
        /// <param name="results">Result list</param>
        /// <returns>Display text</returns>
        string ConvertToText(IList<Result> results);
    }
}
