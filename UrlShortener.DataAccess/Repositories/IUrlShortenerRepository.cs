using System.Threading.Tasks;

namespace UrlShortener.DataAccess.Repositories
{
    /// <summary>Provides storage and retrieval for URL shortener data</summary>
    public interface IUrlShortenerRepository
    {
        /// <summary>Creates (or finds if pre-existing) a shortening for the specified URL, returning its key</summary>
        /// <param name="url">The URL to shorten</param>
        /// <returns>The key for the specified URL</returns>
        Task<string> CreateUrlShortening(string url);

        /// <summary>Gets the URL for the specified key, and increments its redirect count</summary>
        /// <param name="key">The key for which to get the URL</param>
        /// <returns>The URL for the key, or null if not found</returns>
        Task<string> GetRedirectUrl(string key);

        /// <summary>Increments the redirect count for the specified key, for use when the key was found in cache</summary>
        /// <param name="key">The key for which to update the redirect count</param>
        /// <returns>An async task</returns>
        Task IncrementRedirectCount(string key);
    }
}
