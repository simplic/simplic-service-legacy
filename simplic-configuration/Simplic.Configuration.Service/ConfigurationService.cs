using Simplic.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Configuration.Service
{
    public class ConfigurationService : IConfigurationService
    {
        #region Private Members
        private readonly ICacheService cacheService;
        //private readonly ISqlService sqlService;
        #endregion

        public ConfigurationService(ICacheService cacheService)
        {
            this.cacheService = cacheService;
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
        /// <param name="ConfigurationName">Konfigurationswert</param>
        /// <param name="PlugInName">PlugInName</param>
        /// <param name="UserName">Benutzername</param>
        /// <param name="NoCaching">Wenn true, wird kein Cache verwendet</param>
        /// <returns>Wert</returns>
        public T GetValue<T>(string ConfigurationName, string PlugInName, string UserName, bool NoCaching = false)
        {
            var returnValue = cacheService.Get<ConfigurationValue>(ConfigurationValue.GetKeyName(ConfigurationName, PlugInName, UserName));

            if (returnValue == null || NoCaching == true)
            {
                object value = BaseDAL.BaseFunctions.GetValue(PlugInName, UserName, ConfigurationName);

                // If no configuration value exists, try to load a user independent setting
                if (value == null)
                {
                    value = BaseDAL.BaseFunctions.GetValue(PlugInName, "", ConfigurationName);
                }

                returnValue = new ConfigurationValue(ConfigurationName, PlugInName, UserName, value);

                returnValue.Value = CastConfigurationValue<T>(value);

                if (NoCaching == false)
                {
                    cacheService.Set(returnValue);                    
                }
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
        public IEnumerable<ConfigurationValue> GetValues<T>(string plugInName, string userName)
        {

            using (var connection = ConnectionManager.GetOpenPoolConnection<SAConnection>())
            {
                var rawValues = connection.Query("SELECT ConfigValue, ConfigName FROM ESS_MS_Intern_Config WHERE PlugInName = :plugInName AND UserName = :userName", new { plugInName, userName });

                foreach (var rawValue in rawValues)
                {
                    yield return new ConfigurationValue(rawValue.ConfigName, plugInName, userName, CastConfigurationValue<T>(rawValue.ConfigValue));
                }
            }
        }

        /// <summary>
        /// Setzt einen Konfigurationswert
        /// </summary>
        /// <param name="ConfigurationName">Name der Konfiguration</param>
        /// <param name="PlugInName">PlugIn-Name</param>
        /// <param name="UserName">Benutzername</param>
        /// <param name="Value">Wert</param>
        public void SetValue<T>(string ConfigurationName, string PlugInName, string UserName, T Value)
        {
            object value = Value;

            if (value != null)
            {
                if (Value is bool || Value is Boolean)
                {
                    value = Convert.ToInt32(Value).ToString();
                }
            }

            BaseDAL.BaseFunctions.SetValue(PlugInName, UserName, ConfigurationName, value == null ? null : value.ToString());

            ConfigurationValue configValue = CacheManager.Singleton.GetObjectNoException<ConfigurationValue>(ConfigurationValue.GetKeyName(ConfigurationName, PlugInName, UserName));

            if (configValue != null)
            {
                configValue.Value = Value;
            }
            else
            {
                CacheManager.Singleton.AddObject(new ConfigurationValue(ConfigurationName, PlugInName, UserName, Value));
            }
        }
    }
}
