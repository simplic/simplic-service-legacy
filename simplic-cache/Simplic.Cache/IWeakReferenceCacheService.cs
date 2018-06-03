using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Cache
{
    /// <summary>
    /// Cache service interface
    /// </summary>
    public interface IWeakReferenceCacheService
    {
        /// <summary>
        /// Add or replace object to cache
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="key">Unique cache object key</param>Add or replace cache object
        /// <param name="cacheObject">Object to cache</param>
        void Set<T>(string key, T cacheObject);
        
        /// <summary>
        /// Add or replace cache object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="cacheObject">Object to cache</param>
        void Set<T>(T cacheObject) where T : ICacheObject;

        /// <summary>
        /// Get cached object or null
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="key">Unique cache object key</param>
        /// <returns>Cache object if found, else null</returns>
        T Get<T>(string key);

        /// <summary>
        /// Remove object from cache
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="key">Unique cache object key</param>
        void Remove<T>(string key);

        /// <summary>
        /// Clear all objects of a given type
        /// </summary>
        /// <typeparam name="T">Type to clear</typeparam>
        void ClearType<T>();

        /// <summary>
        /// Clear complete cache
        /// </summary>
        void Clear();
    }
}
