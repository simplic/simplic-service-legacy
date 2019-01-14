using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.ConfigurationAnalyzer
{
    public interface IConfigurationTypeAnalyzerService
    {
        IList<Result> Analyze();
        string Name { get; }
    }
}
