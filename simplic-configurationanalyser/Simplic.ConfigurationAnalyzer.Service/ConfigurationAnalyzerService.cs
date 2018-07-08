using Simplic.ConfigurationAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Simplic.ConfigurationAnalyzer.Service
{
    /// <summary>
    /// Configuration analyzer service
    /// </summary>
    public class ConfigurationAnalyzerService
    {
        private IUnityContainer container;

        /// <summary>
        /// Initialize analyzer
        /// </summary>
        /// <param name="container">Container</param>
        public ConfigurationAnalyzerService(IUnityContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Analyze the database
        /// </summary>
        /// <returns>List of results</returns>
        public IList<Result> Analyze()
        {
            var result = new List<Result>();
            var analyzer = container.ResolveAll<IConfigurationTypeAnalyzerService>().ToList();

            foreach (var instance in analyzer)
                result.AddRange(instance.Analyze());

            return result;
        }

        /// <summary>
        /// Convert a result to text
        /// </summary>
        /// <param name="results">Result list</param>
        /// <returns>Display text</returns>
        public string ConvertToText(IList<Result> results)
        {
            var divider = "=== ============================================================= ===";

            var builder = new StringBuilder();
            builder.AppendLine(divider);
            builder.AppendLine($"simplic-configuration-analyzer\r\n @ {GetType().Assembly.FullName}");
            
            // Analyzer text
            builder.AppendLine($"Analyzer: {results.GroupBy(x => x.AnalyzerName).Count()}");
            foreach (var instance in results.GroupBy(x => x.AnalyzerName))
                builder.AppendLine($" > {instance}");


            builder.AppendLine($"Results: {results?.Count.ToString() ?? "<NULL>"} | {DateTime.Now}");
            builder.AppendLine();

            builder.AppendLine(divider);

            builder.AppendLine();
            builder.AppendLine("Result");

            if (results?.Count > 0)
            {
                foreach (var result in results)
                {
                    builder.AppendLine($"{result.ConfigurationType} - {result.Name}");
                    builder.AppendLine(result.Message);
                    builder.AppendLine("---");
                }
            }

            return builder.ToString();
        }
    }
}
