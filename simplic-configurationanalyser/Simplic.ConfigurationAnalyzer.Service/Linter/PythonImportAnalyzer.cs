using Dapper;
using Simplic.Framework.Core;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ConfigurationAnalyzer.Service
{
    /// <summary>
    /// Python import linter
    /// </summary>
    public class PythonImportAnalyzer : IConfigurationTypeAnalyzerService
    {
        private ISqlService sqlService;

        /// <summary>
        /// Initialize analyzer
        /// </summary>
        /// <param name="sqlService">Sql service</param>
        public PythonImportAnalyzer(ISqlService sqlService)
        {
            this.sqlService = sqlService;
        }

        /// <summary>
        /// Analyze python imports
        /// </summary>
        /// <returns></returns>
        public IList<Result> Analyze()
        {
            var results = new List<Result>();

            sqlService.OpenConnection((connection) =>
            {
                var scripts = connection.Query<PythonScript>("select ScriptName as Name, ScriptContent as Code from ESS_MS_Dynamic_Script").ToList();
                scripts.AddRange(connection.Query<PythonScript>("select Name as Name, CodeBehind as Code from ESS_MS_Dynamic_Mask"));

                foreach (var script in scripts)
                {
                    foreach (var line in script.Code.Split(new char[] { '\r', '\n' }))
                    {
                        if (line.StartsWith("import ") || line.StartsWith("from "))
                        {
                            try
                            {
                                GlobalDlrHost.Host.DefaultScope.Execute(line);
                            }
                            catch (Exception ex)
                            {
                                results.Add(new Result()
                                {
                                    AnalyzerName = Name,
                                    Name = $"{script.Name}",
                                    ResultType = ResultType.Error,
                                    Message = $"Error in python import: `{line}`\r\n {ex}",
                                    ConfigurationType = "script code"
                                });
                            }
                        }
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
                return nameof(PythonImportAnalyzer);
            }
        }
    }
}
