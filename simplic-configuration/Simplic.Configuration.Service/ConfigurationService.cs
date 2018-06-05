using Simplic.Cache;
using System;
using System.Collections.Generic;

namespace Simplic.Configuration.Service
{
    public class ConfigurationService : IConfigurationService
    {
        #region Private Members
        private readonly ICacheService cacheService;
        private readonly IConfigurationRepository configurationRepository;
        #endregion

        public ConfigurationService(ICacheService cacheService, IConfigurationRepository configurationRepository)
        {
            this.cacheService = cacheService;
            this.configurationRepository = configurationRepository;
        }

        #region Private Methods

        #region [CastConfigurationValue]
        /// <summary>
        /// Casts a configuration value to a specific type
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <param name="value">Value to cast</param>
        /// <returns>Casted value</returns>
        private T CastConfigurationValue<T>(object value)
        {
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(Boolean))
            {
                value = Convert.ToInt32(value == null ? "0" : value.ToString());
            }
            if (typeof(T) == typeof(bool?) || typeof(T) == typeof(Boolean?))
            {
                if (value != null)
                {
                    value = Convert.ToInt32(value.ToString() == "0");
                }
            }

            try
            {
                return (T)((value == null) ? null : Convert.ChangeType(value, typeof(T)));
            }
            catch
            {
                return default(T);
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// Gibt einen Konfigurationswert zurück
        /// </summary>
        /// <param name="configurationName">Konfigurationswert</param>
        /// <param name="pluginName">PlugInName</param>
        /// <param name="userName">Benutzername</param>
        /// <param name="noCaching">Wenn true, wird kein Cache verwendet</param>
        /// <returns>Wert</returns>
        public T GetValue<T>(string configurationName, string pluginName, string userName, bool noCaching = false)
        {
            var returnValue = cacheService.Get<ConfigurationValue>(
                ConfigurationValue.GetKeyName(configurationName, pluginName, userName));

            if (returnValue == null || noCaching == true)
            {
                var value = configurationRepository.GetValue(pluginName, userName, configurationName);

                // If no configuration value exists, try to load a user independent setting
                if (string.IsNullOrWhiteSpace(value))
                    value = configurationRepository.GetValue(pluginName, "", configurationName);                

                returnValue = new ConfigurationValue(configurationName, pluginName, userName, value);
                returnValue.Value = CastConfigurationValue<T>(value);

                if (noCaching == false)                
                    cacheService.Set(returnValue);                                    
            }

            return (T)returnValue.Value;
        }

        /// <summary>
        /// Get an enumerable of configuration values by its plugin name
        /// </summary>
        /// <typeparam name="T">Expected type</typeparam>
        /// <param name="plugInName">PlugIn-Name</param>
        /// <param name="userName">Current username, should be empty for ignoring</param>
        /// <returns>Enumerable of values</returns>
        public IEnumerable<ConfigurationValue> GetValues<T>(string pluginName, string userName)
        {
            return configurationRepository.GetValues<T>(pluginName, userName);
        }

        /// <summary>
        /// Setzt einen Konfigurationswert
        /// </summary>
        /// <param name="configurationName">Name der Konfiguration</param>
        /// <param name="pluginName">PlugIn-Name</param>
        /// <param name="userName">Benutzername</param>
        /// <param name="value">Wert</param>
        public void SetValue<T>(string configurationName, string pluginName, string userName, T value)
        {
            object _value = value;

            if (_value != null)
            {
                if (value is bool || value is Boolean)
                {
                    _value = Convert.ToInt32(value).ToString();
                }
            }

            configurationRepository.SetValue(pluginName, userName, configurationName,
                _value == null ? null : _value.ToString());            

            var configValue = cacheService.Get<ConfigurationValue>(
                ConfigurationValue.GetKeyName(configurationName, pluginName, userName));

            if (configValue != null)
                configValue.Value = value;
            else
                cacheService.Set(new ConfigurationValue(configurationName, pluginName, userName, value));                            
        }
    }
}
