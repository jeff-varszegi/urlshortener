using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Common.Caching
{
    /// <summary>Implements a layered cache</summary>
    /// <typeparam name="T">The type to cache</typeparam>
    public class CacheManager<T> : ICacheManager<T>
    {
        /// <summary>Caches to use in ascending order</summary>
        private readonly ICache<T>[] _caches;

        public CacheManager(IEnumerable<ICache<T>> caches)
        {
            if (!caches.Any()) throw new ArgumentException("No caches configured for type " + typeof(T).FullName);
            _caches = caches.ToArray();
            Array.Sort(_caches, (x, y) => x.Level.CompareTo(y.Level));
        }

        /// <summary>Adds the specified data to this cache</summary>
        /// <param name="key">The key to use in storing and retrieving the specified data</param>
        /// <param name="data">The data to cache</param>
        public async Task Set(string key, T data)
        {
            if (string.IsNullOrEmpty(key) || object.Equals(data, default)) return; 

            for(int x = _caches.Length - 1; x >= 0; x--) // Set in highest-level/remote cache first to minimize collisions and rework
                await _caches[x].Set(key, data);
        }

        /// <summary>Gets data by the specified key</summary>
        /// <param name="key">The key for which to retrieve the data</param>
        /// <returns>The data for the key, or the specified default if none exists</returns>
        public async Task<T> Get(string key, T defaultValue = default)
        {
            T returnValue = default;
            for(int x = 0; x < _caches.Length; x++)
            {
                returnValue = await _caches[x].Get(key, defaultValue);
                if (!object.Equals(returnValue, defaultValue))
                {
                    // First we must add it to any previous caches which missed
                    for (int y = 0; y < x; y++)
                        _caches[y].Set(key, returnValue); // this can be fire-and-forget

                    return returnValue;
                }
            }

            return returnValue;
        }
    }
}
