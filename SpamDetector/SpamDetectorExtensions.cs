using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Piranha;
using Piranha.Services;
using System;
using System.Linq;
using System.Net.Http;

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
        public static IServiceCollection AddSpamDetector<T>(
            this IServiceCollection services,
            Action<SpamDetectorOptions> options,
            ServiceLifetime scope = ServiceLifetime.Scoped)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), "SpamDetector require valid SpamDetectorOptions");
            }

            if (!services.Any(s => s.ServiceType == typeof(IHttpClientFactory)))
            {
                services.AddHttpClient();
            }

            services.Configure(options);
            App.Modules.Register<SpamDetectorModule>();
            services.Add(new ServiceDescriptor(typeof(ISpamDetector), typeof(T), scope));

            return services;
        }

    }
}
