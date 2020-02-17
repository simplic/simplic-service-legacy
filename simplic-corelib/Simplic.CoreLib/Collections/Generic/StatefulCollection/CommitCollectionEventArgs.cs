using System;

namespace Simplic.Collections.Generic
{
    /// <summary>
    /// Commit event args
    /// </summary>
    public class CommitCollectionEventArgs : EventArgs
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
