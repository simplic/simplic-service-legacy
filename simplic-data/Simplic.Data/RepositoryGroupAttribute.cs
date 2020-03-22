using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Data
{
    /// <summary>
    /// This attribute defines in which group the repository is and which connection to use
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RepositoryGroupAttribute : Attribute
    {
        /// <summary>
        /// Init function the attribute
        /// </summary> 
        /// <param name="groupName">unique Name of the repository group</param>
        public RepositoryGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }

        /// <summary>
        /// Gets or sets the repository group id
        /// </summary>
        public string GroupName { get; set; }
    }
}
