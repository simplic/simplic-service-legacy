using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.DataStack
{
    /// <summary>
    /// Stack timestamp service
    /// </summary>
    public interface IStackTimestampService
    {
        /// <summary>
        /// Set instance data changed timestamp
        /// </summary>
        /// <param name="stackName">Stack name</param>
        /// <param name="instanceDataId">Instance data id</param>
        /// <returns>True if setting was successfull</returns>
        bool SetTimestamp(string stackName, Guid instanceDataId);

        /// <summary>
        /// Set instance data changed timestamp
        /// </summary>
        /// <param name="stackId">Stack id</param>
        /// <param name="instanceDataId">Instance data id</param>
        /// <returns>True if setting was successfull</returns>
        bool SetTimestamp(Guid stackId, Guid instanceDataId);

        /// <summary>
        /// Get all timestamp model
        /// </summary>
        /// <returns>Enumerable of timestamps</returns>
        IEnumerable<InstanceDataTimestampModel> GetAll();

        /// <summary>
        /// Get timestamp by stack name
        /// </summary>
        /// <returns>Timestamp object or null</returns>
        InstanceDataTimestampModel GetByStackName();

        /// <summary>
        /// Get timestamp by stack id
        /// </summary>
        /// <returns>Timestamp object or null</returns>
        InstanceDataTimestampModel GetByStackId();
    }
}
