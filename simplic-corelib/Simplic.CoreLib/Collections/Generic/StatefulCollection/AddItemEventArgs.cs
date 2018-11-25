using System;

namespace Simplic.Collections.Generic
{
    /// <summary>
    /// Add item event args
    /// </summary>
    public class AddItemEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the affected item
        /// </summary>
        public object Item
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether to cancel the action
        /// </summary>
        public bool Cancel
        {
            get;
            set;
        } = false;

        /// <summary>
        /// Gets or sets whether the item was moved from removed items to items
        /// </summary>
        public bool ReaddFromRemovedItems { get; internal set; }
    }
}
