// NOTE: This example use template code  Piranha version 10.0.3  and up  (.NET 6 compact startup / minimal hosting model)
// For template code prior Piranha version 10.0.3 (old .NET startup / hosting model):  see the other example: Startup.cs
using Microsoft.EntityFrameworkCore;
using Piranha;
using Piranha.AttributeBuilder;
using Piranha.AspNetCore.Identity.SQLite;
using Piranha.Data.EF.SQLite;
using Piranha.Manager.Editor;
using Microsoft.AspNetCore.HttpOverrides;
using Zon3.SpamDetector; // <-- 1. Reference to SpamDetector (add source using project reference or add nuget package)

var builder = WebApplication.CreateBuilder(args);

builder.AddPiranha(options =>
{
    /**
     * This will enable automatic reload of .cshtml
     * without restarting the application. However since
     * this adds a slight overhead it should not be
     * enabled in production.
     */
    options.AddRazorRuntimeCompilation = true;

    options.UseCms();
    options.UseManager();

    options.UseFileStorage(naming: Piranha.Local.FileStorageNaming.UniqueFolderNames);
    options.UseImageSharp();
    options.UseTinyMCE();
    options.UseMemoryCache();

    var connectionString = builder.Configuration.GetConnectionString("piranha");
    options.UseEF<SQLiteDb>(db => db.UseSqlite(connectionString));
    options.UseIdentityWithSeed<IdentitySQLiteDb>(db => db.UseSqlite(connectionString));
    options.UseSpamDetector(); // <-- 2. SpamDetector service registered 

    /**
     * Here you can configure the different permissions
     * that you want to use for securing content in the
     * application.
    options.UseSecurity(o =>
    {
        o.UsePermission("WebUser", "Web User");
    });
     */

    /**
     * Here you can specify the login url for the front end
     * application. This does not affect the login url of
     * the manager interface.
    options.LoginUrl = "login";
     */
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UsePiranha(options =>
{    
    // Initialize Piranha
    App.Init(options.Api);

    // Build content types
    new ContentTypeBuilder(options.Api)
        .AddAssembly(typeof(Program).Assembly)
        .Build()
        .DeleteOrphans();

    // Configure Tiny MCE
    EditorConfig.FromFile("editorconfig.json");

    options.UseManager();
    options.UseTinyMCE();
    options.UseIdentity();
    options.UseSpamDetector(); // <-- 3. SpamDetector service configured
});

// 4. Add hook. SpamDetector use OnValidate hook to moderate comments
App.Hooks.Comments.RegisterOnValidate(c =>
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var commentModerator = serviceScope.ServiceProvider.GetRequiredService<AkismetSpamDetectorService>();
    c.IsApproved = commentModerator.ReviewAsync(c).Result.Approved;
});

app.Run();
