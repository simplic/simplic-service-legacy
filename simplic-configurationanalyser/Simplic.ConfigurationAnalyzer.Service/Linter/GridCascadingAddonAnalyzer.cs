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
    /// Grid cascading addon
    /// </summary>
    public class GridCascadingAddonAnalyzer : IConfigurationTypeAnalyzerService
    {
        private ISqlService sqlService;

        /// <summary>
        /// Initialize analyzer
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        public GridCascadingAddonAnalyzer(ISqlService sqlService)
        {
            this.sqlService = sqlService;
        }

        /// <summary>
        /// Analyze grid without cascading addon
        /// </summary>
        /// <returns></returns>
        public IList<Result> Analyze()
        {
            var results = new List<Result>();

            sqlService.OpenConnection((connection) =>
            {
                var grids = connection.Query<GridProfileStatement>(@"
                  SELECT g.Name AS GridName, p.SelectStatement as Statement, p.DisplayName as ProfileName 
                  FROM UI_Grid g 
                  JOIN UI_Grid_Profile p on p.GridId = g.Id
                  WHERE EXISTS (SELECT * FROM ESS_DCC_Stack s WHERE s.StackGridName = g.Name)
                  OR EXISTS (SELECT * FROM ESS_DCC_Structure_Stack s WHERE s.DifGridName = g.Name AND IsNull(s.DifGridName, '') != '')
                  OR EXISTS (SELECT * FROM ESS_DCC_Structure_Stack_Register s WHERE s.DifGridName = g.Name AND IsNull(s.DifGridName, '') != '')");

                foreach (var grid in grids)
                {
                    var statement = grid.Statement.Replace("\r", " ").Replace("\n", " ").ToLower();
                    if (!statement.Contains(" [cascadingaddon] "))
                    {
                        results.Add(new Result()
                        {
                            AnalyzerName = Name,
                            Name = $"{grid.GridName}/{grid.ProfileName}",
                            ResultType = ResultType.Error,
                            Message = "Missing [CascadingAddon]",
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
                return "GridCascadingAddonAnalyzer";
            }
        }
    }
}
