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
    }
}