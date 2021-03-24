using Microsoft.Extensions.DependencyInjection;

using UrlShortener.Common.Caching;
using UrlShortener.Common.Caching.Caches;

namespace UrlShortener.Common
{
    /// <summary>Provides project-specific DI registrations</summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>Registers common classes for DI</summary>
        /// <param name="services">this service collection</param>
        public static void RegisterCommon(this IServiceCollection services)
        {
            // Change these registrations to singletons to save a few objects per request, at the possible expense of easy reconfiguration
            services.AddScoped<ICache<string>, MemoryCache<string>>();
            services.AddScoped<ICacheManager<string>, CacheManager<string>>();
        }
    }
}
