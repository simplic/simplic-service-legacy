using System;
using System.Data;

namespace Simplic.Sql
{
    /// <summary>
    /// Simplic sql service
    /// </summary>
    public interface ISqlService
    {
        /// <summary>
        /// Open new connection and return a value
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="action">Action which will be executed after opening the connection</param>
        /// <param name="name">Connection name</param>
        /// <returns>Func result</returns>
        T OpenConnection<T>(Func<IDbConnection, T> action, string name = "default");

        /// <summary>
        /// Get identity from table
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="tableName">Table name</param>
        /// <param name="name">Connection name</param>
        /// <returns>New identity value</returns>
        T GetIdentity<T>(string tableName, string name = "default");
    }
}
