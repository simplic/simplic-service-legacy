using System.Collections.Generic;

namespace Simplic.Configuration
{
    /// <summary>
    /// Stellt Methoden bereit, um Konfigurationen zu verwalten
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Get an enumerable of configuration values by its plugin name
        /// </summary>
        /// <typeparam name="T">Expected type</typeparam>
        /// <param name="plugInName">PlugIn-Name</param>
        /// <param name="userName">Current username, should be empty for ignoring</param>
        /// <returns>Enumerable of values</returns>
        IEnumerable<ConfigurationValue> GetValues<T>(string plugInName, string userName);

        /// <summary>
        /// Gibt einen Konfigurationswert zurück
        /// </summary>
        /// <param name="ConfigurationName">Konfigurationswert</param>
        /// <param name="PlugInName">PlugInName</param>
        /// <param name="UserName">Benutzername</param>
        /// <param name="NoCaching">Wenn true, wird kein Cache verwendet</param>
        /// <returns>Wert</returns>
        T GetValue<T>(string ConfigurationName, string PlugInName, string UserName, bool NoCaching = false);
        
        /// <summary>
        /// Setzt einen Konfigurationswert
        /// </summary>
        /// <param name="ConfigurationName">Name der Konfiguration</param>
        /// <param name="PlugInName">PlugIn-Name</param>
        /// <param name="UserName">Benutzername</param>
        /// <param name="Value">Wert</param>
        void SetValue<T>(string ConfigurationName, string PlugInName, string UserName, T Value);
    }
}
