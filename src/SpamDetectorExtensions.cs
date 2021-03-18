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
        /// Adds the SpamDetector module.
        /// </summary>
        /// <param name="services">The current service collection</param>
        /// <param name="scope">The optional service scope. Default is singleton</param>
        /// <returns>The services</returns>
        public static IServiceCollection AddSpamDetector<T>(this IServiceCollection services, ServiceLifetime scope = ServiceLifetime.Scoped)
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

            // Add the Manager config service and the module service
            services.AddScoped<SpamDetectorLocalizer>();
            services.AddScoped<SpamDetectorConfigService>();

            // Finally add the module itself as a service
            services.Add(new ServiceDescriptor(typeof(ISpamDetector), typeof(T), scope));

            return services;
        }

        /// <summary>
        /// Uses the SpamDetector module.
        /// </summary>
        /// <param name="builder">The application builder</param>
        /// <returns>The builder</returns>
        public static IApplicationBuilder UseSpamDetector(this IApplicationBuilder builder)
        {
            // Define a file provider for static files 
            return builder.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = builder.ApplicationServices.GetRequiredService<EmbeddedFileProvider>(),
                RequestPath = "/manager/assets"
            });
        }

        /// <summary>
        /// Adds the SpamDetector options.
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
