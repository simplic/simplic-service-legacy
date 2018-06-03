using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Reflection
{
    /// <summary>
    /// Provides functions to wotk with C# namespaces
    /// </summary>
    public class NamespaceHelper
    {
        /// <summary>
        /// Get all namespaces in an assembly
        /// </summary>
        /// <param name="asm">Instance of assembly</param>
        /// <returns>List with namespaces</returns>
        public static IEnumerable<string> GetNamespacesInAssembly(Assembly asm)
        {
            return asm.GetTypes()
                            .Select(t => t.Namespace)
                            .Distinct();
        }
    }
}
