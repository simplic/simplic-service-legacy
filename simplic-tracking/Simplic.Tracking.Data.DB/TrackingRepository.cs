using System.Collections.Generic;
using Dapper;
using System.Linq;
using Simplic.Sql;

namespace Simplic.Tracking.Data.DB
{    
    /// <summary>
    /// Implements database repository for the tracking system
    /// </summary>
    public class TrackingRepository : ITrackingRepository
    {
        private const string TableName = "Tracking";
        private const string TrackingEntryTableName = "Tracking_Entry";
        private readonly ISqlService sqlService;        

        public TrackingRepository(ISqlService sqlService)
        {
            this.sqlService = sqlService;
        }

        /// <summary>
        /// Gets the next free tracking id
        /// </summary>
        /// <returns>A new id of the tracking table</returns>
        public long GetNewTrackingId()
        {
            return sqlService.OpenConnection( (connection) => {
                return connection.Query<long>($"Select get_identity('{TableName}');")
                    .FirstOrDefault();
            });            
        }

        /// <summary>
        /// Gets the tracking object by a given id
        /// </summary>
        /// <param name="trackingId">The id of the tracking</param>
        /// <returns>The <see cref="Tracking"/> with the given id</returns>
        public Tracking GetTracking(long trackingId)
        {
            return sqlService.OpenConnection((connection) => {
                return connection.Query<Tracking>($"Select * From {TableName} Where TrackingId = :trackingId;", 
                    new { trackingId }).FirstOrDefault();
            });
        }

        /// <summary>
        /// Gets all tracking entries objects by a given tracking id
        /// </summary>
        /// <param name="trackingId">the id of the tracking</param>
        /// <returns>Enumerable of tracking entries</returns>
        public IEnumerable<TrackingEntry> GetTrackingEntriesByTrackingId(long trackingId)
        {
            return sqlService.OpenConnection((connection) => {
                return connection.Query<TrackingEntry>($"Select * From {TrackingEntryTableName} Where TrackingId = :trackingId;",
                    new { trackingId });
            });            
        }

        /// <summary>
        /// Writes a tracking to the database
        /// </summary>
        /// <param name="tracking">the tracking to save</param>
        public bool SaveTracking(Tracking tracking)
        {
            return sqlService.OpenConnection((connection) => {

                var affectedRows = connection.Execute($"Insert Into {TableName} " +
                    $" (Id, TrackingTimestamp, TableName, DataGuid, UserId, Type) On Existing Update Values " +
                    $" (:Id, :TrackingTimestamp, :TableName, :DataGuid, :UserId, :Type);", tracking);

                return affectedRows > 0;
            });            
        }

        /// <summary>
        /// Writes a tracking entry to the database
        /// </summary>
        /// <param name="newEntries">The tracking entries to save</param>
        public bool SaveEntries(IList<TrackingEntry> newEntries)
        {
            return sqlService.OpenConnection((connection) => {

                foreach (var entry in newEntries)
                {
                    var sql = $"Insert Into {TrackingEntryTableName} " +
                        $" (FieldName, OldValue{entry.Type}, NewValue{entry.Type}, TrackingId, Type) " +
                        $" On Existing Update Values (:FieldName, :OldValue, :NewValue, :TrackingId, :Type)";
                    var affectedRows = connection.Execute(sql, entry);

                    if (affectedRows <= 0)
                        return false;
                }

                return true;
            });
        }
    }
}
