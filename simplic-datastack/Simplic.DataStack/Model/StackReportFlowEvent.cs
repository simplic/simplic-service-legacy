using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.DataStack
{
    /// <summary>
    /// Flow event configuration
    /// </summary>
    public class StackReportFlowEvent
    {
        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the icon name
        /// </summary>
        public Guid IconId
        {
            get;
            set;
        }
    }
}
