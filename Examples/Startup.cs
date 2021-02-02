
/*
 * Copyright (c) .NET Foundation and Contributors
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *
 * http://github.com/tidyui/coreweb
 *
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Piranha;
using Piranha.Data.EF.SQLite;
using Piranha.AspNetCore.Identity.SQLite;
using Piranha.AttributeBuilder;
using Piranha.Local;
using System;
using Zon3.SpamDetector;

// Please Note: This is a MODIFIED startup.cs from the RazorWeb template. It adds all the needed code to run the
// SpamDetector module. All lines added to the standard RazorWeb template are preceeded by a comment with the word 'Added: '.
// There are additional steps to take to get the module up and running but this is all the actual code required to write/add.
// See full details on SpamDetector GitHub page: https://github.com/jensbrak/SpamDetector
// An appsettings.json file with a sample sections is also provided in the same directory as this file. 
namespace RazorWeb
{
    public class Startup
    {
		// Added: For convenience, keep a reference to the configuration so we can use it when configuring services
        private readonly IConfiguration _config;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="configuration">The current configuration</param>
        public Startup(IConfiguration configuration)
        {
			// Added: set the local reference to the configuration
            _config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
			// Added: Configure SpamDetector module with it's settings as provided by the configuration
            services.AddHttpClient();
            services.Configure<SpamDetectorOptions>(_config.GetSection(SpamDetectorOptions.Identifier));

            // Piranha configuration
            services.AddPiranha(options =>
            {
                options.AddRazorRuntimeCompilation = true;

                options.UseFileStorage(naming: Piranha.Local.FileStorageNaming.UniqueFolderNames);
                options.UseImageSharp();
                options.UseManager();
                options.UseTinyMCE();
                options.UseMemoryCache();
				
                options.UseEF<SQLiteDb>(db =>
                    db.UseSqlite("Filename=./piranha.razorweb.db"));
                options.UseIdentityWithSeed<IdentitySQLiteDb>(db =>
                    db.UseSqlite("Filename=./piranha.razorweb.db"));

                options.UseSecurity(o =>
                {
                    o.UsePermission("Subscriber");
                });
				
				// Added: Load the SpamDetector service
                options.UseSpamDetector<AkismetSpamDetector>(o =>
				{
                    _config.GetSection(SpamDetectorOptions.Identifier).Bind(o)
				});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApi api)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Initialize Piranha
            App.Init(api);

            // Configure cache level
            App.CacheLevel = Piranha.Cache.CacheLevel.Full;

            // Register custom components
            App.Blocks.Register<RazorWeb.Models.Blocks.MyGenericBlock>();
            App.Blocks.Register<RazorWeb.Models.Blocks.RawHtmlBlock>();
            App.Modules.Manager().Scripts.Add("~/assets/custom-blocks.js");
            App.Modules.Manager().Styles.Add("~/assets/custom-blocks.css");

            // Build content types
            new ContentTypeBuilder(api)
                .AddAssembly(typeof(Startup).Assembly)
                .Build()
                .DeleteOrphans();

            // Configure Editor
            Piranha.Manager.EditorConfig.FromFile("editorconfig.json");

            // Middleware setup
            app.UsePiranha(options => {
                options.UseManager();
                options.UseTinyMCE();
                options.UseIdentity();
            });

            // Added: Add custom hook for SpamDetector module. This links the module functionality with the Piranha framework
			// by defining a delegate that takes a comment c, gets the SpamDetector service, feeds it the comment and modifies
			// the incomming comment with the outcome of the SpamDetector (ie Akismet API) validation.
            App.Hooks.Comments.RegisterOnValidate(c =>
            {
                using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                ISpamDetector commentModerator = serviceScope.ServiceProvider.GetRequiredService<ISpamDetector>();
                c.IsApproved = commentModerator.ReviewAsync(c).Result.Approved;
            });
        }
    }
}
