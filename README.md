# SpamDetector
| Server | Status |
| ------ | ------ |
| AppVeyor | [![Build status](https://ci.appveyor.com/api/projects/status/x55tk8vtffvt354b?svg=true)](https://ci.appveyor.com/project/jensbrak/spamdetector) |
| NuGet | [![Nuget](https://img.shields.io/nuget/v/Zon3.SpamDetector)](https://www.nuget.org/packages/Zon3.SpamDetector) |

Spam Detection Module for PiranhaCMS using Akismet API.

# About
A [PiranhaCMS](https://github.com/PiranhaCMS "Piranha CMS at GitHub") module that use Akismet to validate comments submitted to a Piranha Page or Post. This is done by providing Akismet with details from the comment and the site using the module. If the comment is considered to be spam by Akismet, it will not be approved by Piranha. Comments in Piranha Manager are either approved or pending which means (when the module is active): all comments marked approved has been verified as non-spam by the module and all comments marked as pending is spam (and can be removed).

_Please note: using the module as a Comment Validation Hook will override the Manager Config setting for Approve Comments. This means that regardless of that setting a comment will be approved (ie published) if it is not considered spam. If considered spam, the comment will not be approved (ie pending)._

# Dependencies
* `Microsoft.Extensions.Http`
* `Piranha` version 9

_Note: Piranha version 10 is planned to have hooks redesigned, forcing this module to be redesigned too when version 10 is released. Take this into account if using this module. (See referenced issue under Further reading section)._

# Prerequisites
* A solution or project using PiranhaCMS (see https://piranhacms.org/)
* An Akismet Developer API key (see https://akismet.com/development/api)

# Further reading
* Piranha Modules: 
	* https://piranhacms.org/docs/extensions/modules
	* https://github.com/PiranhaCMS/piranha.modules
* Piranha Hooks: 
	* https://piranhacms.org/docs/application/hooks
* Akismet API documentation:
    * https://akismet.com/development/api/#detailed-docs
    * https://akismet.com/development/api/#comment-check
* Related Piranha issues:
    * Redesign of Hooks: https://github.com/PiranhaCMS/piranha.core/issues/1236

# Demo
Please go ahead and try it out by posting a comment with a greeting on my blog. It runs PiranhaCMS with this module active. If your comment is directly visible it's been classified by akismet as non-spam and approved by the SpamDetector module as a valid comment. If not, stop spamming! (Or report a bug to me ;) )

My PiranhaCMS demo site: https://zon3.se 

# Usage
See Code snippets below for pointers.

## Code
1. Add a reference to `Zon3.SpamDetector` in your Piranha project
1. In `Startup.cs` register reqired services and hooks: 
    1. Register `SpamDetector` service
    1. Register a Comment validation hook calling `SpamDetector.ReviewAsync(Comment c)` that validates comments 

## Settings
The module adds a section to the Manager with the settings available:

* Enabled (default: `true`): If false, module will not send requests and leave comments unreviewed
* IsTest (default: `true`): If true, all requests are marked 'test'. See Note 1.
* Spam Api Url: the complete URL to the API to use for spam detection
* Site Url: the base URL of the site the comments are posted on. See Note 2. 
* Site Language (default: `"en-US"`): The language of the site the comments are posted on. See Note 2.
* Site Encoding (default: `"UTF8"`): The encoding of the site the comments are posted on. See Note 2.
* User Role (default: `"guest"`): The name of the user role comments are posted as. See Note 2.

_Note 1: The effect of `IsTest` set to `true` is to send API requests without affecting Akismet heuristics. Akismet will, however, do a proper assessment of the comment and return the result. As per documentation there are defined values that can be used to test positive Spam detection (see Akismet documentation). When SpamModule is configured and tested this value should be set to `false`_

_Note 2: These settings reflect the site SpamDetector is used from. They are sent along each comment to Akismet to aid the analysis. While SpamDetector will run without these settings Akismet encourage them to be sent. In the future SpamDetector module might get these values from Piranha automatically._

![image](https://user-images.githubusercontent.com/52660827/110434774-8511c380-80b2-11eb-8d69-71ea4f91533e.png)

The settings are persisted in the Piranha database as `Param` values (same as Piranha use for configuration settings).

# Code snippets
## Configuring services
```csharp
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPiranha(options =>
            {
                // (Standard Piranha setup code removed for brevity)

				options.UseSpamDetector<AkismetSpamDetector>();

```

## Configuring middleware & addding comment hook
```csharp
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApi api)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Initialize Piranha
            App.Init(api);

            // Build content types
            new ContentTypeBuilder(api)
                .AddAssembly(typeof(Startup).Assembly)
                .Build()
                .DeleteOrphans();

            // Configure Tiny MCE
            EditorConfig.FromFile("editorconfig.json");

            // Middleware setup
            app.UsePiranha(options => {
                options.UseManager();
                options.UseTinyMCE();
                options.UseIdentity();
		options.UseSpamDetector<AkismetSpamDetector>(); // <-- SpamDetector using Akismet implementation
            });

            // SpamDetector use OnValidate hook to moderate comments
            App.Hooks.Comments.RegisterOnValidate(c =>
            {
                using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var spamDetector = serviceScope.ServiceProvider.GetRequiredService<SpamDetector>();
		
		// SpamDetector will call Akismet API to review the comment and then return the result
                var reviewResult = spamDetector.ReviewAsync(c).Result;
		
		// This updates the comment submitted with the result of the antispam review
                c.IsApproved = reviewResult.Approved;
            });
        }
```			
