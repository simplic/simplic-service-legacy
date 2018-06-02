using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Cache
{
    /// <summary>
    /// Cacheable object
    /// </summary>
    public interface ICacheObject
    {
        /// <summary>
        /// Gets the cache key
        /// </summary>
        string CacheKey
        {
            get;
        }
    }
}
