# SpamDetector
Basic Spam Detection Module for PiranhaCMS using Akismet API

# Description
A basic module that use Akismet to validate comments submitted to a Piranha Page or Post. This is done by providing Akismet with details from the comment and the site using the module. If the comment is considered spam by Akismet, it will not be approved by Piranha. The comment instead will then be marked Pending in Piranha Manager.

_Please note: using the module as a Comment Validation Hook will override the Manager Config setting for Approve Comments. This means that regardless of that setting a comment will be approved (ie published) if it is not considered spam. If considered spam, the comment will not be approved (ie pending)._

# Dependencies
* `Microsoft.Extensions.Http`
* `Piranha` (version 8.x, see notes below)

_Note 1: current version of SpamDetector is designed using Piranha version 8.x. However, in order for the module to work properly, it require fixes scheduled for the version 9 of Piranha (see issues #1338 and #1347). Either try this module out with Piranha with the fixes applied - or wait for version 9. But... _

_Note 2: Piranha version 9 will have hooks redesigned, forcing this module to be redesigned too (when version 9 is released). Take this into account if trying this module._

# Prerequisites
* A solution or project using PiranhaCMS (see https://piranhacms.org/)
* An Akismet Developer API key (see https://akismet.com/development/api)

# Further reading
* Piranha Modules: https://piranhacms.org/docs/extensions/modules
* Piranha Hooks: https://piranhacms.org/docs/application/hooks
* Piranha Hooks related issues:
    * https://github.com/PiranhaCMS/piranha.core/issues/1347
    * https://github.com/PiranhaCMS/piranha.core/issues/1236
* Piranha Comments related issues:
    * https://github.com/PiranhaCMS/piranha.core/issues/1338
    * https://github.com/PiranhaCMS/piranha.core/issues/1236
* Akismet API documentation:
    * https://akismet.com/development/api/#detailed-docs
    * https://akismet.com/development/api/#comment-check

# Usage / setup
See Code snippets at the end for shortened examples. Basically, do this:

## Code
1. Add a reference to `Zon3.SpamDetector` in your Piranha project
1. In `Startup.cs` register reqired services and hooks: 
    1. Register `IHttpClientFactory` service
    1. Register `SpamDetector` service using `SpamDetectorOptions` for settings
    1. Register `SpamDetector` as a Comment hook after Piranha has been setup

## Settings
1. Update the settings file (appSettings.json) with a section for SpamDetector (ie `"SpamDetector": { }`)  
1. Add at least the options for `SpamApiUrl`, `IsTest` and `SiteUrl` (see notes below why the latter two are recommended but not required)

The class `SpamDetectorOptions` defines the options that can be set for SpamDetector. These options are read from settings file. The options available are:

* `Enabled` (optional, default: `true`): If false, module will not send requests and leave comments unreviewed
* `SpamApiUrl` (Required): the complete URL to the API to use for spam detection
* `SiteUrl` (optional): the base URL of the site the comments are posted on. See Note 1 below. 
* `SiteLanguage` (optional, default: `"en-US"`): The language of the site the comments are posted on 
* `SiteEncoding` (optional, default: `"UTF8"`): The encoding of the site the comments are posted on
* `UserRole` (optional, default: `"guest"`): The name of the user role comments are posted as
* `IsTest` (optional, default: `true`): If true, all requests are marked 'test'. See Note 2 below.

_Note 1: While SpamDetector will run without `SiteUrl` defined, it is highly recommended to set it to get reliable results. However, while testing it shouldn't matter (see `IsTest`)._

_Note 2: While optional, the value of `IsTest` will have to be changed to `false` eventually. The reason for having to do this explicitly is to prevent undesired live requests to the API while setting up and testing._

# Design and roadmap
## Design thoughts
Primarly designed to help understanding how modules and hooks work in Piranha, without complicating things too much but still by using Piranha and .NET Core features available.
It does work preventing spam too but that was more of a side effect of the project.
That being said, some thoughts on reusability was put into the main classes (`SpamDetector` and `CommentReview`). The idea is:

1. Define a base class `SpamDetector` that implements the interface `ISpamDetector`. It is just a simple wrapper to make an API-call and some signature definitions to be used for spam detection.
1. Derive the base class with an actual implementation `AkismetSpamDetector` using an API (Akismet). This is done by 
     1. Implementing `async Task<HttpRequestMessage> GetSpamRequestMessageAsync(Comment comment)` which transform a Piranha `Comment` to an API-request for Akismet
     2. Implementing `async Task<CommentReview> GetCommentReviewFromResponse(HttpResponseMessage response)` which transform the API-resonse from Akismet back to something to be used by Piranha
1. Register a comment hook that intercept comments before they are saved. Use it to check if it's spam or not and change the field `IsApproved` of the `Comment` accordingly
1. Register a module definition to show it's installed

## Roadmap and ideas
Some things I'd like to improve (Pull requests are most welcome btw!):

* Use a second API and implement it (any suggestions as for which API?). Adjust the module if needed.
* Adjust it for Piranha version 9.
* Make nuget package (preferably using automation). Not meaningful to do before version 9 is released.
* ...?...

# Code snippets
## Registering services
```csharp
        public void ConfigureServices(IServiceCollection services)
        {
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
