using System;

namespace Simplic.Collections.Generic
{
    /// <summary>
    /// Item removed event args
    /// </summary>
    public class ItemRemovedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the affected item
        /// </summary>
        public object Item
        {
            get;
            set;
        }
    }
}