using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.DataStack
{
    /// <summary>
    /// base python parameter
    /// </summary>
    public abstract class ReportPythonParameter
    {
        /// <summary>
        /// Gets or sets the current instance data id
        /// </summary>
        public Guid instance_data_id { get; set; }

        /// <summary>
        /// Gets or sets the stack report configuration
        /// </summary>
        public StackReport stack_report { get; set; }
    }
}
