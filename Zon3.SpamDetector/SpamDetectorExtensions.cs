using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Piranha;
using System.Linq;
using System.Net.Http;
using Zon3.SpamDetector.Localization;
using Zon3.SpamDetector.Services;

namespace Zon3.SpamDetector
{
    /// <summary>
    /// Service extensions.
    /// </summary>
    public static class SpamDetectorExtensions
    {
        /// <summary>
        /// Adds the SpamDetectorService module.
        /// </summary>
        /// <param name="services">The current service collection</param>
        /// <returns>The services</returns>
        public static IServiceCollection AddSpamDetector(this IServiceCollection services)
        {
            // Register the module to Piranha
            App.Modules.Register<SpamDetectorModule>();

            // Add external module services, if needed
            if (services.All(s => s.ServiceType != typeof(IHttpClientFactory)))
            {
                services.AddHttpClient();
            }

            // Add module dependency service instances
            services.AddSingleton(_ => new EmbeddedFileProvider(typeof(SpamDetectorModule).Assembly, "Zon3.SpamDetector.assets.dist"));
            services.AddSingleton(s => new SpamDetectorMarkdownService(s.GetRequiredService<EmbeddedFileProvider>(), "doc"));

            // Add the internal services
            services.AddScoped<SpamDetectorLocalizer>();
            services.AddScoped<SpamDetectorConfigService>();

            // Finally add the module itself as a service
            services.AddScoped<AkismetSpamDetectorService>();

            return services;
        }

        /// <summary>
        /// Uses the SpamDetectorService module.
        /// </summary>
        /// <param name="builder">The application builder</param>
        /// <returns>The builder</returns>
        public static IApplicationBuilder UseSpamDetector(this IApplicationBuilder builder)
        {
            return builder.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = builder.ApplicationServices.GetRequiredService<EmbeddedFileProvider>(),
                RequestPath = "/manager/assets"
            });
        }

        /// <summary>
        /// Adds the SpamDetectorService options.
        /// </summary>
        /// <param name="builder">The MVC builder</param>
        /// <returns>The builder</returns>
        public static IMvcBuilder AddSpamDetectorOptions(this IMvcBuilder builder)
        {
            return builder
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();
        }
    }
}
