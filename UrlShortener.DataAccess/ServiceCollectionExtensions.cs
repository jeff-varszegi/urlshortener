using Microsoft.Extensions.DependencyInjection;
using UrlShortener.DataAccess.Repositories;

namespace UrlShortener.DataAccess
{
    /// <summary>Provides project-specific DI registrations</summary>
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>Registers data-access classes for DI</summary>
        /// <param name="services">this service collection</param>
        public static void RegisterDataAcess(this IServiceCollection services)
        {
            services.AddScoped<IUrlShortenerRepository, PostgresUrlShortenerRepository>();
        }
    }
}
