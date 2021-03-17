using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Piranha;
using System.Linq;
using System.Net.Http;
using Zon3.SpamDetector.Localization;
using Zon3.SpamDetector.Services;

namespace Zon3.SpamDetector
{
    public static class SpamDetectorExtensions
    {
        /// <summary>
        /// Adds the services for the SpamDetector service.
        /// </summary>
        /// <param name="services">The current service collection</param>
        /// <param name="scope">The optional service scope. Default is singleton</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddSpamDetector<T>(this IServiceCollection services, ServiceLifetime scope = ServiceLifetime.Scoped)
        {
            App.Modules.Register<SpamDetectorModule>();

            if (services.All(s => s.ServiceType != typeof(IHttpClientFactory)))
            {
                services.AddHttpClient();
            }

            // Add module dependency service instances
            services.AddSingleton(s => new EmbeddedFileProvider(typeof(SpamDetectorModule).Assembly, "Zon3.SpamDetector.assets.dist"));
            services.AddSingleton(s => new SpamDetectorMarkdownService(s.GetRequiredService<EmbeddedFileProvider>(), "doc"));

            // Add the Manager config service and the module service
            services.AddScoped<SpamDetectorLocalizer>();
            services.AddScoped<SpamDetectorConfigService>();
            services.Add(new ServiceDescriptor(typeof(ISpamDetector), typeof(T), scope));

            return services;
        }

        public static IApplicationBuilder UseSpamDetector<T>(this IApplicationBuilder builder)
        {
            return builder.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = builder.ApplicationServices.GetRequiredService<EmbeddedFileProvider>(),
                RequestPath = "/manager/assets"
            });
        }

        public static void MapSpamDetector(this IEndpointRouteBuilder builder)
        {
            builder.MapRazorPages();
        }

        public static IMvcBuilder AddSpamDetectorOptions(this IMvcBuilder builder)
        {
            return builder
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();
        }
    }
}
