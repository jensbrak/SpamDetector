# SpamDetector
Basic Spam Detection Module for PiranhaCMS using Akismet API

# Description
A basic module that use Akismet to validate comments submitted to a Piranha Page or Post. This is done by providing Akismet with details from the comment and the site using the module. If the comment is considered spam by Akismet, it will not be approved. The comment will then be marked Pending in Piranha Manager.

Please note: using the module as a Comment Validation Hook will override the Manager Config setting for Approve Comments. This means that regardless of that setting a comment will be approved (ie published) if it is not considered spam. If considered spam, the comment will not be approved (ie pending).

# Dependencies
* `Microsoft.Extensions.Http`
* `Piranha` (version 8.x, see notes below)

_Note 1: current version of SpamDetector is designed using Piranha version 8.x. However, in order for the module to work properly, it require fixes made scheduled to be released with version 9 (see issues #1338 and #1347). Either try this module out with those fixes applied to Piranha or wait for version 9._

_Note 2: Piranha version 9 will have hooks redesigned, forcing this module to be redesigned when version 9 is released. Take this into account if trying this module._

# Prerequisites
* A solution or project using PiranhaCMS (see https://piranhacms.org/)
* An Akismet Developer API key (see https://akismet.com/development/api)

# Usage
See Code snippets below for example.

## Code
1. Add a reference to `Zon3.SpamDetector` in your Piranha project
1. In `Startup.cs` register reqired services and hooks: 
    1. Register `IHttpClientFactory` service
    1. Register `SpamDetector` service using `SpamDetectorOptions` for settings
    1. Register `SpamDetector` as a Comment hook after Piranha has been)

## Settings
The class `SpamDetectorOptions` defines the options that can be set for SpamDetector. The options available are:

* `Enabled` (optional, default: `true`): If false, module will not send requests and leave comments unreviewed
* `SpamApiUrl` (Required): the complete URL to the API to use for spam detection
* `SiteUrl` (optional): the base URL of the site the comments are posted on. See Note 1. 
* `SiteLanguage` (optional, default: `"en-US"`): The language of the site the comments are posted on 
* `SiteEncoding` (optional, default: `"UTF8"`): The encoding of the site the comments are posted on
* `UserRole` (optional, default: `"guest"`): The name of the user role comments are posted as
* `IsTest` (optional, default: `true`): If true, all requests are marked 'test'. See Note 2.

_Note 1: While SpamDetector will run without `SpamApiUrl` defined, it is highly recommended to set it to get reliable results. However, while testing it shouldn't matter (see `IsTest`)._

_Note 2: While optional, the value of `IsTest` will have to be changed to `false` eventually. The reason for having to do this explicitly is to prevent undesired live requests to the API while setting up and testing._


# Code snippets
## Registering services
```csharp
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.Configure<SpamDetectorOptions>(_config.GetSection(SpamDetectorOptions.Identifier));

            services.AddPiranha(options =>
            {
                // (Standard Piranha setup code removed for brevity)

                options.UseSpamDetector<AkismetSpamDetector>(o => 
                    _config.GetSection(SpamDetectorOptions.Identifier).Bind(o));            

```

## Adding Comment Hook
```csharp
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApi api)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // (Standard Piranha configuration code removed for brevity)

            App.Hooks.Comments.RegisterOnValidate(c =>
            {
                using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                ISpamDetector commentModerator = serviceScope.ServiceProvider.GetRequiredService<ISpamDetector>();
                c.IsApproved = !commentModerator.ReviewAsync(c).Result.IsSpam;
            });
        }
```

## Sample appsettings.json section
A minimal section for SpamDetecor would be:

```json
    "SpamDetector": {
        "SpamApiUrl": "https://<yourakismetapikeyhere>.rest.akismet.com/1.1/comment-check",
        "SiteUrl": "https://<yourawesomepiranhasitehere>",
        "IsTest": false
    }
```

_Note: Only set `IsTest` to true when confident everything is properly setup and working_