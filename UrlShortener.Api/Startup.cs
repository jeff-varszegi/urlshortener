using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

using UrlShortener.Common;
using UrlShortener.DataAccess;

namespace UrlShortener
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton(Configuration);
            services.Configure<AppSettings>(Configuration);
            services.AddOptions();

            var loggerConfig = new LoggerConfiguration()
#if !DEBUG                
                .MinimumLevel.Information()
#endif            
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .WriteTo.Async(_ => _.Console())
                .CreateLogger();
            services.AddLogging(_ => _.AddSerilog(loggerConfig));

            // Initialize the memory cache (used e.g. by UrlShortener.Common.Caching.Caches.MemoryCache)
            services.AddMemoryCache(cache => { cache.SizeLimit = 10000000; }); // adjust based on projections, and/or push to config

            // Call DI hooks in referenced projects
            services.RegisterCommon();
            services.RegisterDataAcess();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
