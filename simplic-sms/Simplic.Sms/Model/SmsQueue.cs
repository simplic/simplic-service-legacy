using System;

namespace Simplic.Sms
{
    /// <summary>
    /// Sms Queue object
    /// </summary>
    public class SmsQueue
    {
        /// <summary>
        /// Gets or sets the Id of the sms queue object
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the sms queue object
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the text of the sms queue object
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the status of the sms queue object
        /// </summary>
        public SmsStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the provider response of the sms queue object
        /// </summary>
        public string ProviderResponse { get; set; }

        /// <summary>
        /// Gets or sets the created time of the sms queue object
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Gets or sets the sent time of the sms queue object
        /// </summary>
        public DateTime? SentTime { get; set; }
    }
}