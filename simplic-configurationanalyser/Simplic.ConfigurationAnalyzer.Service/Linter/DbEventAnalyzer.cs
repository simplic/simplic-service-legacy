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
    /// Db event analyzer
    /// </summary>
    public class DbEventAnalyzer : IConfigurationTypeAnalyzerService
    {
        private ISqlService sqlService;

        /// <summary>
        /// Initialize analyzer
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        public DbEventAnalyzer(ISqlService sqlService)
        {
            this.sqlService = sqlService;
        }

        public IList<Result> Analyze()
        {
            var results = new List<Result>();

            sqlService.OpenConnection((connection) =>
            {
                var events = connection.Query<string>("select event_name from sysevent");
                foreach (var evt in events)
                {
                    results.Add(new Result()
                    {
                        AnalyzerName = Name,
                        Name = $"{evt}",
                        ResultType = ResultType.Warning,
                        Message = "Events are not allowed",
                        ConfigurationType = "db-event"
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
                return nameof(DbEventAnalyzer);
            }
        }
    }
}
