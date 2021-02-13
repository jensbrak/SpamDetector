using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Piranha;
using Piranha.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace Zon3.SpamDetector
{
    public static class SpamDetectorStartupExtensions
    {
        /// <summary>
        /// Adds the CommentReview module if simple startup is used.
        /// </summary>
        /// <param name="serviceBuilder">The service builder</param>
        /// <param name="scope">The optional service scope. Default is singleton</param>
        /// <returns>The updated builder</returns>
        public static PiranhaServiceBuilder UseSpamDetector<T>(
            this PiranhaServiceBuilder serviceBuilder,
            Action<SpamDetectorOptions> options,
            ServiceLifetime scope = ServiceLifetime.Scoped)
        {
            serviceBuilder.Services.AddSpamDetector<T>(options, scope);

            return serviceBuilder;
        }
    }
}
