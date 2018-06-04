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
