using Simplic.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Cache.Service
{
    /// <summary>
    /// WeakReference cache service implementation
    /// </summary>
    public class WeakReferenceCacheService : IWeakReferenceCacheService
    {
        private IDictionary<string, WeakReference> cache;
        private object _lock = new object();

        /// <summary>
        /// Initialize cache service
        /// </summary>
        public WeakReferenceCacheService()
        {
            cache = new Dictionary<string, WeakReference>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Get cached object or null
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="key">Unique cache object key</param>
        /// <returns>Cache object if found, else null</returns>
        public T Get<T>(string key)
        {
            lock (_lock)
            {
                if (string.IsNullOrWhiteSpace(key)) throw new Exception("Cache key must not be null or white space");

                var cacheKey = GetTypeBasedKey(key, typeof(T));

                if (cache.ContainsKey(cacheKey))
                {
                    var reference = cache[cacheKey];
                    if (reference.IsAlive)
                        return (T)reference.Target;
                    else
                        Remove<T>(key);
                }

                return default(T);
            }
        }

        /// <summary>
        /// Add or replace cache object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="cacheObject">Object to cache</param>
        public void Set<T>(T cacheObject) where T : ICacheObject
        {
            lock (_lock)
            {
                if (cacheObject == null) throw new ArgumentNullException(nameof(cacheObject));
                if (string.IsNullOrWhiteSpace(cacheObject.CacheKey)) throw new Exception("Cache key must not be null or white space");

                var cacheKey = GetTypeBasedKey(cacheObject.CacheKey, typeof(T));

                cache[cacheKey] = new WeakReference(cacheObject);
            }
        }

        /// <summary>
        /// Add or replace object to cache
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="key">Unique cache object key</param>Add or replace cache object
        /// <param name="cacheObject">Object to cache</param>
        public void Set<T>(string key, T cacheObject)
        {
            lock (_lock)
            {
                if (string.IsNullOrWhiteSpace(key)) throw new Exception("Cache key must not be null or white space");

                var cacheKey = GetTypeBasedKey(key, typeof(T));

                cache[cacheKey] = new WeakReference(cacheObject);
            }
        }

        /// <summary>
        /// Remove object from cache
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="key">Unique cache object key</param>
        public void Remove<T>(string key)
        {
            lock (_lock)
            {
                if (string.IsNullOrWhiteSpace(key)) throw new Exception("Cache key must not be null or white space");

                var cacheKey = GetTypeBasedKey(key, typeof(T));

                if (cache.ContainsKey(cacheKey))
                    cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Clear all objects of a given type
        /// </summary>
        /// <typeparam name="T">Type to clear</typeparam>
        public void ClearType<T>()
        {
            lock (_lock)
            {
                // Important: nullable checl
                var toRemove = cache.Where(x => x.Value?.Target.GetType() == typeof(T)).ToList();

                foreach (var entry in toRemove)
                    cache.Remove(entry);
            }
        }

        /// <summary>
        /// Clear complete cache
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                cache = new Dictionary<string, WeakReference>(StringComparer.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Generate type bsaed key
        /// </summary>
        /// <param name="key">Unique key within a type</param>
        /// <param name="cacheType">Type to cache</param>
        /// <returns>Unique cache key</returns>
        private string GetTypeBasedKey(string key, Type cacheType)
        {
            return $"{key}_{cacheType.FullName}";
        }
    }
}
