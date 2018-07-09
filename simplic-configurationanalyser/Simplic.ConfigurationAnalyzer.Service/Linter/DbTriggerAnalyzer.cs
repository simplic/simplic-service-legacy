using Dapper;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ConfigurationAnalyzer.Service
{
    /// <summary>
    /// Db trigger analyzer
    /// </summary>
    public class DbTriggerAnalyzer : IConfigurationTypeAnalyzerService
    {
        private ISqlService sqlService;

        /// <summary>
        /// Initialize analyzer
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        public DbTriggerAnalyzer(ISqlService sqlService)
        {
            this.sqlService = sqlService;
        }

        public IList<Result> Analyze()
        {
            var results = new List<Result>();

            sqlService.OpenConnection((connection) =>
            {
                var events = connection.Query<string>("select trigger_name from systrigger WHERE trigger_name IS NOT NULL");
                foreach (var evt in events)
                {
                    results.Add(new Result()
                    {
                        AnalyzerName = Name,
                        Name = $"{evt}",
                        ResultType = ResultType.Warning,
                        Message = "Trigger are not allowed",
                        ConfigurationType = "db-trigger"
                    });
                }
            });

            return results;
        }

        /// <summary>
        /// Gets the analyzer name
        /// </summary>
        public string Name
        {
            get
            {
                return nameof(DbTriggerAnalyzer);
            }
        }
    }
}
