using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Cache
{
    /// <summary>
    /// Contains the simplic cache scope for parent and child caching
    /// </summary>
    public class CacheScope
    {
        private IDictionary<string, object> cache;

        /// <summary>
        /// Initialize new scope
        /// </summary>
        public CacheScope()
        {
            cache = new Dictionary<string, object>();
        }

        /// <summary>
        /// Get item from cache
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="key">Item key</param>
        /// <param name="create">Delegate for creating new items</param>
        /// <returns>New or existing item</returns>
        public T Get<T>(string key, Func<string, T> create)
        {
            return GetByScope<T>(GetScope(), key, create);
        }

        /// <summary>
        /// Get or create item
        /// </summary>
        /// <typeparam name="T">Item type</typeparam>
        /// <param name="scope">Scope instance</param>
        /// <param name="key">Item key</param>
        /// <param name="create">Delegate for creating new items</param>
        /// <returns>New or existing item</returns>
        private T GetByScope<T>(CacheScope scope, string key, Func<string, T> create)
        {
            if (scope.cache.ContainsKey(key))
                return (T)scope.cache[key];

            var item = default(T);

            if (create != null)
                item = create(key);

            if (item != null)
            {
                scope.cache[key] = item;
                return item;
            }

            return default(T);
        }

        /// <summary>
        /// Get scope to use. If no parent exists, the current scope will be used
        /// </summary>
        /// <returns>Scope instance</returns>
        public CacheScope GetScope()
        {
            if (Parent == null)
                return this;

            var scopeList = new List<CacheScope>();
            var lastParent = this.Parent;

            while (lastParent != null)
            {
                var nextParent = lastParent.Parent;

                if (nextParent == null || scopeList.Contains(nextParent))
                    break;

                scopeList.Add(nextParent);
                lastParent = nextParent;
            }

            return lastParent;
        }

        /// <summary>
        /// Clear cache
        /// </summary>
        public void Clear()
        {
            var scope = GetScope();
            scope.cache = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets the parent scope
        /// </summary>
        public CacheScope Parent { get; set; }
    }
}
