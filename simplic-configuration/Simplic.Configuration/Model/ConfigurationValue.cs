using Simplic.Cache;

namespace Simplic.Configuration
{
    /// <summary>
    /// ConfigurationValue
    /// </summary>
    public class ConfigurationValue : ICacheObject
    {
        private string configurationName;
        private string plugInName;
        private string userName;
        private object value;

        /// <summary>
        /// Constructor to create a new configuration value
        /// </summary>
        /// <param name="configName">Configuration name</param>
        /// <param name="plugInName">Plugin name</param>
        /// <param name="userName">User name</param>
        /// <param name="value">Configuration value</param>
        public ConfigurationValue(string configName, string plugInName, string userName, object value)
        {
            this.configurationName = configName;
            this.plugInName = plugInName;
            this.userName = userName;
            this.value = value;
        }


        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }


        /// <summary>
        /// Gets the configuration name
        /// </summary>
        public string ConfigName
        {
            get
            {
                return configurationName;
            }
        }

        public string CacheKey
        {
            get { return GetKeyName(configurationName, plugInName, userName).ToLower().Trim(); }
        }


        /// <summary>
        /// Generiert den eindeutigen Key eines ConvifurationValue - ICacheable
        /// </summary>
        /// <param name="configName">Configuration name</param>
        /// <param name="plugInName">Plugin name</param>
        /// <param name="userName">User name</param>
        /// <returns>Key name</returns>
        public static string GetKeyName(string configurationName, string plugInName, string userName)
        {
            return (configurationName + plugInName + userName).ToLower().Trim();
        }
    }
}