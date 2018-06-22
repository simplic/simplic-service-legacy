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
    }
}
