using System.Threading.Tasks;

namespace UrlShortener.Common.Caching
{
    /// <summary>Provides storage and retrieval of the specified type by a string key</summary>
    public interface ICache<T>
    {
        // TODO: Consider whether it would be helpful to implement CacheEntry<T> as a data wrapper
        // TODO: Consider add removal behavior as necessary

        /// <summary>The level or order of checking this cache layer, lower-numbered first</summary>
        public int Level { get; } // TODO: Push this into configuration

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
