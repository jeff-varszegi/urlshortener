using System.Threading.Tasks;

namespace UrlShortener.Common.Caching
{
    // Note: This basic layered-cache implementation is intended to show how one would easily add Memcached, Redis etc. to the mix

    /// <summary>Manages data storage and retrieval in layered caches</summary>
    public interface ICacheManager<T>
    {
        /// <summary>Adds the specified data to this cache</summary>
        /// <param name="key">The key to use in storing and retrieving the specified data</param>
        /// <param name="data">The data to cache</param>
        Task Set(string key, T data);

        /// <summary>Gets data by the specified key</summary>
        /// <param name="key">The key for which to retrieve the data</param>
        /// <returns>The data for the key, or the specified default if none exists</returns>
        Task<T> Get(string key, T defaultValue = default);
    }
}
