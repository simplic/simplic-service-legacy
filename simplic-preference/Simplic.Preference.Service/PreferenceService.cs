using System;
using System.Collections.Generic;

namespace Simplic.Preference.Service
{
    /// <summary>
    /// Preference service holds a list of key value pairs (Dictionary)
    /// </summary>
    public class PreferenceService : IPreferenceService
    {
        /// <summary>
        /// Gets or sets the preferences
        /// </summary>
        public IDictionary<string, string> Preferences { get; set; }

        /// <summary>
        /// Gets or sets the ini section
        /// </summary>
        public string IniSection { get; set; }

        /// <summary>
        /// Gets or sets the studio path
        /// </summary>
        public string StudioPath { get; set; }

        /// <summary>
        /// Gets or sets app data path
        /// </summary>
        public string AppDataPath { get; set; }

        /// <summary>
        /// Gets or sets the bin directory
        /// </summary>
        public string BinDirectoryPath { get; set; }

        /// <summary>
        /// Gets or sets the application session id
        /// </summary>
        public Guid ApplicationSessionId { get; set; }

        /// <summary>
        /// Gets or sets the given application arguments
        /// </summary>
        public IDictionary<string, string> Arguments { get; set; }
    }
}
