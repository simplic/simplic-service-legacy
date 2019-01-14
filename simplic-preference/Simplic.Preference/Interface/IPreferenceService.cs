using System;
using System.Collections.Generic;

namespace Simplic.Preference
{
    /// <summary>
    /// Preference service holds a list of key value pairs (Dictionary)
    /// </summary>
    public interface IPreferenceService
    {
        /// <summary>
        /// Gets or sets the preferences
        /// </summary>
        IDictionary<string, string> Preferences { get; set; }

        /// <summary>
        /// Gets or sets the ini section
        /// </summary>
        string IniSection { get; set; }

        /// <summary>
        /// Gets or sets the studio path
        /// </summary>
        string StudioPath { get; set; }

        /// <summary>
        /// Gets or sets app data path
        /// </summary>
        string AppDataPath { get; set; }

        /// <summary>
        /// Gets or sets the bin directory
        /// </summary>
        string BinDirectoryPath { get; set; }

        /// <summary>
        /// Gets or sets the application session id
        /// </summary>
        Guid ApplicationSessionId { get; set; }

        /// <summary>
        /// Gets or sets the given application arguments
        /// </summary>
        IDictionary<string, string> Arguments { get; set; }
    }
}