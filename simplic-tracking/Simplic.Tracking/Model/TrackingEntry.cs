namespace Simplic.Tracking
{
    /// <summary>
    /// Represents a change of one property
    /// </summary>
    public class TrackingEntry
    {
        #region [Private Member]

        private long id;
        private string fieldName;
        private object oldValue;
        private object newValue;
        private long trackingId;
        private string type;
        #endregion

        #region [Constructor]

        /// <summary>
        /// Initializes a new TrackingEntry by a given FieldName, 2 Values and a trackingId
        /// </summary>
        /// <param name="newFieldName"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="newTrackingId"></param>
        public TrackingEntry(string newFieldName, object oldValue, object newValue, long newTrackingId)
        {
            this.fieldName = newFieldName;
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.trackingId = newTrackingId;
        }

        #endregion

        #region [Public Member]

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public long Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        /// <summary>
        /// Gets or sets the FieldName
        /// </summary>
        public string FieldName
        {
            get
            {
                return fieldName;
            }

            set
            {
                fieldName = value;
            }
        }

        /// <summary>
        /// Gets or sets the OldValue
        /// </summary>
        public object OldValue
        {
            get
            {
                return oldValue;
            }

            set
            {
                oldValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the NewValue
        /// </summary>
        public object NewValue
        {
            get
            {
                return newValue;
            }

            set
            {
                newValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the TrackinId
        /// </summary>
        public long TrackingId
        {
            get
            {
                return trackingId;
            }

            set
            {
                trackingId = value;
            }
        }

        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        public string Type
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

        #endregion
    }
}
