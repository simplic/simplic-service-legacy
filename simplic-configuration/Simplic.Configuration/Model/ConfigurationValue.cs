using Simplic.Cache;

namespace Simplic.Configuration
{
    /// <summary>
    /// Konfigurationswert
    /// </summary>
    public class ConfigurationValue : ICacheObject
    {
        private string configName;
        private string plugInName;
        private string userName;
        private object value;

        /// <summary>
        /// Neues Configuration-Wert erstellen
        /// </summary>
        /// <param name="ConfigName">Name der Einstellung</param>
        /// <param name="PlugInName">PlugIn-Name</param>
        /// <param name="UserName">UserName</param>
        /// <param name="Value">Wert</param>
        public ConfigurationValue(string ConfigName, string PlugInName, string UserName, object Value)
        {
            configName = ConfigName;
            plugInName = PlugInName;
            userName = UserName;
            value = Value;
        }

        public void OnRemove()
        {

        }

        /// <summary>
        /// Generiert den eindeutigen Key eines ConvifurationValue - ICacheable
        /// </summary>
        /// <param name="ConfigurationName"></param>
        /// <param name="PlugInName"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        internal static string GetKeyName(string ConfigurationName, string PlugInName, string UserName)
        {
            return (ConfigurationName + PlugInName + UserName).ToLower().Trim();
        }

        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Eindeutiger Schlüssel
        /// </summary>
        public string Key
        {
            get { return GetKeyName(configName, plugInName, userName); }
        }

        /// <summary>
        /// Gets the configuration name
        /// </summary>
        public string ConfigName
        {
            get
            {
                return configName;
            }
        }

        public string CacheKey
        {
            get { return nameof(ConfigurationValue); }
        }
    }
}