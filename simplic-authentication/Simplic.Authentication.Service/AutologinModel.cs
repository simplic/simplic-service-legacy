using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Authentication.Service
{
    /// <summary>
    /// Gets or sets the autologin model
    /// </summary>
    internal class AutologinModel
    {
        /// <summary>
        /// Gets or sets the current username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the current domain
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the current hash
        /// </summary>
        public string Hash { get; set; }
    }
}
