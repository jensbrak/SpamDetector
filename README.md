# SpamDetector
Basic Spam Detection Module for PiranhaCMS using Akismet API

# Description
A simple module using Akismet to register to validate comments submitted. This is done by providing Akismet with details from the comment and the site using the module. If the comment is considered spam by Akismet, it will not be approved. The comment will then be marked Pending in Piranha Manager.

Please note: using the module as a Comment Validation Hook will override the Manager Config setting for Approve Comments. This means that regardless of that setting a comment will be approved (ie published) if it is not considered spam. If considered spam, the comment will not be approved (ie pending).

# Dependencies
* `Microsoft.Extensions.Http`
* `Piranha` (version 8.x)

# Prerequisites
* A solution or project using PiranhaCMS (see https://piranhacms.org/)
* An Akismet Developer API key (see https://akismet.com/development/api)

# Usage
1. Add a reference to `Zon3.SpamDetector`
1. Register `IHttpClientFactory` service
1. Register `SpamDetector` service using `SpamDetectorOptions` for settings
    1. Configure `SpamDetectorOptions` and then use it to load settings
    1. Add required keys in `appsetttings.json`
    1. Use the options when registering the `SpamDetector` service 
    1. ... or just Use lambda directly 
1. Register `SpamDetector` as a Comment hook

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
```json
    "SpamDetector": {
        "SpamApiUrl": "https://<yourakismetapikeyhere>.rest.akismet.com/1.1/comment-check",
        "SiteUrl": "https://<yourawesomepiranhasitehere>",
        "IsTest": true
    }
```

Please note: the `IsTest` property tells Akismet that any call is a test. Change to false only after you verified your API key and the functionality of the module together with your setup. Read more about Akismet on their page (see Prerequisites)