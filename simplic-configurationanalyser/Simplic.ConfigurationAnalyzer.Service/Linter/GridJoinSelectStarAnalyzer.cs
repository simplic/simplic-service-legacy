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
    /// Grid join select star
    /// </summary>
    public class GridJoinSelectStarAnalyzer : IConfigurationTypeAnalyzerService
    {
        private ISqlService sqlService;

        /// <summary>
        /// Initialize analyzer
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        public GridJoinSelectStarAnalyzer(ISqlService sqlService)
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

            sqlService.OpenConnection((connection) =>
            {
                var grids = connection.Query<GridProfileStatement>("SELECT g.Name AS GridName, p.SelectStatement as Statement, p.DisplayName as ProfileName FROM UI_Grid g JOIN UI_Grid_Profile p on p.GridId = g.Id");
                foreach (var grid in grids)
                {
                    var statement = grid.Statement.Replace("\r", " ").Replace("\n", " ").ToLower();
                    if (statement.Contains("*") && statement.Contains(" join "))
                    {
                        results.Add(new Result()
                        {
                            AnalyzerName = Name,
                            Name = $"{grid.GridName}/{grid.ProfileName}",
                            ResultType = ResultType.Error,
                            Message = "Join and select * is not allowed",
                            ConfigurationType = "grid"
                        });
                    }
                    else if (statement.Contains(".*") || statement.Contains(" *"))
                    {
                        results.Add(new Result()
                        {
                            AnalyzerName = Name,
                            Name = $"{grid.GridName}/{grid.ProfileName}",
                            ResultType = ResultType.Warning,
                            Message = "Please avoid using select * ...",
                            ConfigurationType = "grid"
                        });
                    }
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
                return "GridOrderByAnalyzer";
            }
        }
    }
}
