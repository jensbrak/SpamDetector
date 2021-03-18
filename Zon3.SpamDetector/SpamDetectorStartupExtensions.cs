using Microsoft.Extensions.DependencyInjection;
using Piranha;
using Piranha.AspNetCore;

namespace Zon3.SpamDetector
{
    /// <summary>
    /// The startup extensions for SpamDetectorService.
    /// </summary>
    public static class SpamDetectorStartupExtensions
    {
        /// <summary>
        /// Uses the SpamDetectorService and its Piranha Manager services if simple startup is used.
        /// </summary>
        /// <param name="serviceBuilder">The service builder</param>
        /// <param name="scope">The optional service scope. Default is singleton</param>
        /// <returns>The updated builder</returns>
        public static PiranhaServiceBuilder UseSpamDetector(this PiranhaServiceBuilder serviceBuilder)
        {
            serviceBuilder.Services.AddLocalization(options =>
                options.ResourcesPath = "Resources"
            );
            //serviceBuilder.Services.AddControllersWithViews();
            serviceBuilder.Services.AddRazorPages()
                .AddSpamDetectorOptions();
            serviceBuilder.Services.AddSpamDetector();

            return serviceBuilder;
        }

        /// <summary>
        /// Uses the SpamDetectorService if simple startup is used.
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
