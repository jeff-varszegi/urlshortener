using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace UrlShortener.Common.Caching.Caches
{
    /// <summary>A wrapper for the class library memory cache implementation</summary>
    public class MemoryCache<T> : ICache<T>
    {
        /// <summary>A prefix to avoid cache collisions in the face of use of the same cache</summary>
        private static readonly string CachePrefix = typeof(MemoryCache<T>).FullName + ":";

        /// <summary>The underlying caching provider</summary>
        private readonly IMemoryCache _cache;

        /// <summary>Settings to use when cachin objects</summary>
        private readonly MemoryCacheEntryOptions _cacheSettings;

        /// <summary>Constructs a new instance</summary>
        /// <param name="cache">The underlying cache to use</param>
        public MemoryCache(IMemoryCache cache, IOptionsSnapshot<AppSettings> settings)
        {
            _cache = cache ?? throw new ArgumentNullException("cache");

            int expirationSeconds = settings.Value.MemoryCacheExpirationSeconds.RestrictTo(600, int.MaxValue);
            _cacheSettings = new MemoryCacheEntryOptions().SetSize(1).SetSlidingExpiration(TimeSpan.FromSeconds(expirationSeconds));
        }

        /// <summary>The order of checking this cache layer (which should be checked before out-of-process caches)</summary>
        public int Level { get { return 1; } }

        /// <summary>Adds the specified data to this cache</summary>
        /// <param name="key">The key to use in storing and retrieving the specified data</param>
        /// <param name="data">The data to cache</param>
        public async Task Set(string key, T data)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Invalid key");
            else if (object.Equals(data, default)) return;
            _cache.Set(CachePrefix + key.Trim(), data, _cacheSettings);
        }

        /// <summary>Gets data by the specified key</summary>
        /// <param name="key">The key for which to retrieve the data</param>
        /// <returns>The data for the key, or the specified default if none exists</returns>
        public async Task<T> Get(string key, T defaultValue = default)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Invalid key");
            return _cache.TryGetValue(CachePrefix + key.Trim(), out T value) ? value : defaultValue;
        }
    }
}
