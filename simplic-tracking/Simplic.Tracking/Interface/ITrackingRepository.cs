using System.Collections.Generic;

namespace Simplic.Tracking
{
    /// <summary>
    /// Contains the signatures of database methods
    /// </summary>
    public interface ITrackingRepository
    {
        /// <summary>
        /// Gets the next free tracking id
        /// </summary>
        /// <returns>a new id of the tracking table</returns>
        long GetNewTrackingId();

        /// <summary>
        /// Gets the tracking with the given id
        /// </summary>
        /// <param name="trackingId">the id of the tracking</param>
        /// <returns>the tracking with the given id</returns>
        Tracking GetTracking(long trackingId);

        /// <summary>
        /// Gets all tracking entries with the given tracking id
        /// </summary>
        /// <param name="trackingId">the id of the tracking</param>
        /// <returns>enumerable of tracking entries</returns>
        IEnumerable<TrackingEntry> GetTrackingEntriesByTrackingId(long trackingId);

        /// <summary>
        /// Writes a tracking to the database
        /// </summary>
        /// <param name="tracking">the tracking to save</param>
        bool SaveTracking(Tracking tracking);

        /// <summary>
        /// Writes a tracking entry to the database
        /// </summary>
        /// <param name="newEntries">the tracking entries to save</param>
        bool SaveEntries(IList<TrackingEntry> newEntries);
    }
}
