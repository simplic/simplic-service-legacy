using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ConfigurationAnalyzer.Service
{
    /// <summary>
    /// Grid order by analyzer
    /// </summary>
    public class GridOrderByAnalyzer : IConfigurationTypeAnalyzerService
    {
        private ISqlService sqlService;

        /// <summary>
        /// Initialize analyzer
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        public GridOrderByAnalyzer(ISqlService sqlService)
        {
            this.sqlService = sqlService;
        }

        /// <summary>
        /// Analyze grid order by
        /// </summary>
        /// <returns></returns>
        public IList<Result> Analyze()
        {
            var results = new List<Result>();

            

            return results;
        }

        /// <summary>
        /// Gets the analyzer name
        /// </summary>
        public string Name
        {
            get
            {
                return "GridOrderByAnalyzer";
            }
        }
    }
}
