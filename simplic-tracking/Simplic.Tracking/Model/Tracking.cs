using System;
using System.Collections.Generic;

namespace Simplic.Tracking
{
    /// <summary>
    /// Represents a change of all properties of an object with timestamp and current user
    /// </summary>
    public class Tracking
    {
        #region [Private Member]

        private IList<TrackingEntry> entries;
        private long id;
        private DateTime trackingTimestamp;
        private string tableName;
        private Guid dataGuid;
        private int userId;
        private short type;
        private readonly ITrackingService trackingService;

        #endregion

        public Tracking(string tableName, DateTime dateTime, int userId, Guid dataGuid, short type)
        {
            entries = new List<TrackingEntry>();
            trackingService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ITrackingService>();

            this.tableName = tableName;
            this.TrackingTimestamp = dateTime;
            this.userId = userId;
            this.dataGuid = dataGuid;
            this.type = type;
        }

        public long Id
        {
            get
            {
                if (id == 0) id = trackingService.GetNewTrackingId();
                return id;
            }
            set
            {
                id = value;
            }
        }

        public DateTime TrackingTimestamp
        {
            get
            {
                return trackingTimestamp;
            }
            set
            {
                trackingTimestamp = value;
            }
        }

        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                tableName = value;
            }
        }

        public Guid DataGuid
        {
            get
            {
                return dataGuid;
            }
            set
            {
                dataGuid = value;
            }
        }

        public IList<TrackingEntry> Entries
        {
            get
            {
                return entries;
            }
        }

        public int UserId
        {
            get
            {
                return userId;
            }

            set
            {
                userId = value;
            }
        }

        public short Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }
    }
}
