using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Piranha;
using Piranha.Services;
using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Zon3.SpamDetector.Models;
using Microsoft.Extensions.FileProviders;
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
        public static IServiceCollection AddSpamDetector(this IServiceCollection services)
        {
            App.Modules.Register<SpamDetectorModule>();

            if (services.All(s => s.ServiceType != typeof(IHttpClientFactory)))
            {
                services.AddHttpClient();
            }

            services.AddScoped<SpamDetectorConfigService>();

            return services;
        }

        public static IApplicationBuilder UseSpamDetector(this IApplicationBuilder builder)
        {
            return builder.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new EmbeddedFileProvider(typeof(SpamDetectorModule).Assembly, "Zon3.SpamDetector.assets.dist"),
                RequestPath = "/manager/assets"
            });
        }

        public static void MapSpamDetector(this IEndpointRouteBuilder builder)
        {
            builder.MapRazorPages();
        }
    }
}
