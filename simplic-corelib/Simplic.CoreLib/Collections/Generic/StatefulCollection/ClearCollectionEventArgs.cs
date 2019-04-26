using System;

namespace Simplic.Collections.Generic
{
    /// <summary>
    /// Before the collection is cleared
    /// </summary>
    public class ClearCollectionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets whether to cancel the action
        /// </summary>
        public bool Cancel
        {
            get;
            set;
        } = false;
    }
}
