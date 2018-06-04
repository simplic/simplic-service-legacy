using System;

namespace Simplic.Tracking
{
    /// <summary>
    /// Contains the methods that implement the business logic
    /// </summary>
    public interface ITrackingService : ITrackingRepository
    {
        /// <summary>
        /// Compares the values of the properties of 2 objects with the same type.
        /// For each value that is different a new tracking entry is made.
        /// </summary>
        /// <param name="oldObj">the old Object to compare</param>
        /// <param name="newObj">the new Object to compare</param>
        /// <param name="tableName">database tablename of the objects</param>
        /// <param name="instanceDataGuid">Guid of the data record</param>
        void TrackChanges(object oldObj, object newObj, string tableName, Guid instanceDataGuid);
    }
}
