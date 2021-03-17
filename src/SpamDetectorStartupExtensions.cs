﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Piranha;
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
        public static PiranhaServiceBuilder UseSpamDetector<T>(this PiranhaServiceBuilder serviceBuilder, ServiceLifetime scope = ServiceLifetime.Scoped)
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

        public static PiranhaApplicationBuilder UseSpamDetector<T>(this PiranhaApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Builder.UseSpamDetector<T>();
            applicationBuilder.Builder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapPiranhaManager();
            });

            return applicationBuilder;
            }
    }
}
