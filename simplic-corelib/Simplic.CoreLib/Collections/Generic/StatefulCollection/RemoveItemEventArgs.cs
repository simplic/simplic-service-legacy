using System;

namespace Simplic.Collections.Generic
{
    /// <summary>
    /// Remove item arguments
    /// </summary>
    public class RemoveItemEventArgs : EventArgs
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
        /// Gets or sets whether the item is removed from uncommitted items
        /// </summary>
        public bool RemoveFromNewItems { get; internal set; }
    }
}
