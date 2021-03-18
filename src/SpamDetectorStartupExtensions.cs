using Microsoft.Extensions.DependencyInjection;
using Piranha;
using Piranha.AspNetCore;

namespace Zon3.SpamDetector
{
    /// <summary>
    /// The startup extensions for SpamDetector.
    /// </summary>
    public static class SpamDetectorStartupExtensions
    {
        /// <summary>
        /// Uses the SpamDetector and its Piranha Manager services if simple startup is used.
        /// </summary>
        /// <param name="serviceBuilder">The service builder</param>
        /// <param name="scope">The optional service scope. Default is singleton</param>
        /// <returns>The updated builder</returns>
        public static PiranhaServiceBuilder UseSpamDetector<T>(this PiranhaServiceBuilder serviceBuilder,
            ServiceLifetime scope = ServiceLifetime.Scoped)
        {
            serviceBuilder.Services.AddLocalization(options =>
                options.ResourcesPath = "Resources"
            );
            serviceBuilder.Services.AddControllersWithViews();
            serviceBuilder.Services.AddRazorPages()
                .AddSpamDetectorOptions();
            serviceBuilder.Services.AddSpamDetector<T>();

            return serviceBuilder;
        }

        /// <summary>
        /// Uses the SpamDetector if simple startup is used.
        /// </summary>
        /// <param name="applicationBuilder">The application builder</param>
        /// <returns>The updated builder</returns>
        public static PiranhaApplicationBuilder UseSpamDetector(this PiranhaApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Builder.UseSpamDetector();
            return applicationBuilder;
        }
    }
}
