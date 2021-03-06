using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Piranha;
using Piranha.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Piranha.AspNetCore;

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
        public static PiranhaServiceBuilder UseSpamDetector(this PiranhaServiceBuilder serviceBuilder)
        {
            serviceBuilder.Services.AddControllersWithViews();
            serviceBuilder.Services.AddRazorPages();
            serviceBuilder.Services.AddSpamDetector();

            return serviceBuilder;
        }

        public static PiranhaApplicationBuilder UseSpamDetector(this PiranhaApplicationBuilder applicatoinBuilder)
        {
            applicatoinBuilder.Builder.UseSpamDetector();
            applicatoinBuilder.Builder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapPiranhaManager();
            });

            return applicatoinBuilder;
        }
    }
}
