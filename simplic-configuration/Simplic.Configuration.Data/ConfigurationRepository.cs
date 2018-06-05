using Dapper;
using Simplic.Sql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.Configuration.Data
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private const string TableName = "ESS_MS_Intern_Config";
        private readonly ISqlService sqlService;

        public ConfigurationRepository(ISqlService sqlService)
        {
            this.sqlService = sqlService;
        }

        #region Private Methods
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

        /// <summary>
        /// Gets a configuration value
        /// </summary>
        /// <param name="plugInName">Plugin name</param>
        /// <param name="userName">User name</param>
        /// <param name="configurationName">Configuration name</param>
        /// <returns>Configuration value</returns>
        public string GetValue(string pluginName, string userName, string configurationName)
        {
            var sql = $"SELECT ConfigValue FROM {TableName} WHERE " +
                $" PlugInName LIKE :pluginName and UserName LIKE :username and ConfigName LIKE :configurationName ";

            return sqlService.OpenConnection((connection) => {
                return connection.Query<string>(sql, new { pluginName, userName, configurationName })
                    .FirstOrDefault();
            });
        }

        /// <summary>
        /// Sets a configuration value (saves in the db)
        /// </summary>
        /// <param name="pluginName">Plugin name</param>
        /// <param name="userName">User name</param>
        /// <param name="configurationName">Configuration name</param>
        /// <param name="configurationValue">Configuration value</param>
        public void SetValue(string pluginName, string userName, string configurationName, string configurationValue)
        {
            // Wenn es eine Benutzerabhängige Konfiguration ist und noch kein Wert existiert, dann versuchen wir den datensatz zu kopieren.
            if (string.IsNullOrWhiteSpace(userName) == false)
            {
                if (GetValue(pluginName, userName, configurationName) == null)
                {
                    var sql = $"INSERT INTO {TableName}(PlugInName, UserName, ConfigName, ConfigValue, ContentType, " +
                        $" IsEditable, UserCanOverwrite) " +
                        $" (SELECT PlugInName, :userName, ConfigName, :configurationValue, ContentType, IsEditable, " +
                        $" UserCanOverwrite FROM {TableName} WHERE PlugInName = :pluginName AND " +
                        $" ConfigName = :configurationName AND UserName = '')";

                    sqlService.OpenConnection((connection) => {

                        var affectedRows = connection.Execute(sql, 
                            new { pluginName, userName, configurationName, configurationValue });

                        return affectedRows > 0;
                    });
                }                               
            }

            if (GetValue(pluginName, userName, configurationName) == null)
            {
                sqlService.OpenConnection((connection) => {
                    var sql = $"INSERT INTO {TableName}(PlugInName, UserName, ConfigName, ConfigValue) " +
                        $" values(:pluginName, :userName, :configurationName, :configurationValue)";

                    var affectedRows = connection.Execute(sql,
                           new { pluginName, userName, configurationName, configurationValue });

                    return affectedRows > 0;
                });
            }
            else
            {
                sqlService.OpenConnection((connection) => {
                    var sql = $"UPDATE {TableName} SET ConfigValue = :configurationValue WHERE PlugInName = :pluginName " +
                            $" AND UserName = :userName AND ConfigName = :configurationName";

                    var affectedRows = connection.Execute(sql,
                           new { pluginName, userName, configurationName, configurationValue });

                    return affectedRows > 0;
                });
            }
        }

        /// <summary>
        /// Gets a list configuration values
        /// </summary>
        /// <typeparam name="T">Expected type</typeparam>
        /// <param name="plugInName">Plugin name</param>
        /// <param name="userName">User name</param>
        /// <returns>A list configuration values</returns>
        public IEnumerable<ConfigurationValue> GetValues<T>(string plugInName, string userName)
        {
            var rawValues = sqlService.OpenConnection((connection) => {

                return connection.Query($"SELECT ConfigValue, ConfigName FROM {TableName} " +
                     $" WHERE PlugInName = :plugInName AND UserName = :userName", new { plugInName, userName });
            });

            foreach (var rawValue in rawValues)
            {
                yield return new ConfigurationValue(rawValue.ConfigName, plugInName, userName,
                    CastConfigurationValue<T>(rawValue.ConfigValue));
            }
        }
    }
}
