using System;

namespace Simplic.DataStack
{
    /// <summary>
    /// Instance data timestamp model
    /// </summary>
    public class InstanceDataTimestampModel
    {
        /// <summary>
        /// Gets or sets the stack guid
        /// </summary>
        public Guid StackGuid { get; set; }

        /// <summary>
        /// Gets or sets the last changed instance data id
        /// </summary>
        public Guid LastInstanceDataGuid { get; set; }

        /// <summary>
        /// Gets or sets the change datetime
        /// </summary>
        public DateTime ChangeDateTime { get; set; }

        /// <summary>
        /// Gets or sets the change username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the change user id
        /// </summary>
        public int UserId { get; set; }
    }
}
