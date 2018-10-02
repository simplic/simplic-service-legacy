using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.DataStack
{
    /// <summary>
    /// Parameter class which will be used in the post script
    /// </summary>
    public class PostReportPythonParameter : ReportPythonParameter
    {
        /// <summary>
        /// Gets or sets the report result as blob
        /// </summary>
        public byte[] report_blob { get; set; }

        /// <summary>
        /// Gets or sets the report result file extension
        /// </summary>
        public string file_extension { get; set; }
    }
}
