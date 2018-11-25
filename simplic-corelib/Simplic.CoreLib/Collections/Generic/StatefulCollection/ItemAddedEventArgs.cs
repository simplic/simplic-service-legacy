using System;

namespace Simplic.Collections.Generic
{
    /// <summary>
    /// Item added event arguments
    /// </summary>
    public class ItemAddedEventArgs : EventArgs
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