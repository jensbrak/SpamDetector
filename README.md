# SpamDetector| Server | Status |
| ------ | ------ |
| AppVeyor | [![Build status](https://ci.appveyor.com/api/projects/status/x55tk8vtffvt354b?svg=true)](https://ci.appveyor.com/project/jensbrak/spamdetector) |
| NuGet | [![Nuget](https://img.shields.io/nuget/v/Zon3.SpamDetector)](https://www.nuget.org/packages/Zon3.SpamDetector) |
# About## SpamDetector
Project page: [zon3.se/spamdetector](https://zon3.se/spamdetector)

## Description
Piranha CMS provides a feature to allow visitors to comment posts and pages. 
This module adds automatic spam detection functionality for these comments.
Once configured, the module will intercept all comments before they are published. 
If considered spam, the comment is marked as not approved, effectively stopping it from being published.
If not considered spam, the comment is marked as approved and is published.
# Dependencies* `Microsoft.Extensions.Http`
* `Piranha` version 9
* An Akismet API key

_Note: Piranha version 10 is planned to have hooks redesigned, forcing this module to be redesigned too when version 10 is released. Take this into account if using this module. (See referenced issue under Further reading section)._
# DemoPlease go ahead and try it out by posting a comment with a greeting on my blog. It runs PiranhaCMS with this module active. If your comment is directly visible it's been classified by akismet as non-spam and approved by the SpamDetector module as a valid comment. If not, stop spamming! (Or report a bug to me ;) )

My PiranhaCMS demo site: [zon3.se](https://zon3.se)
# InstallationSee Instructions below and/or the example [`Startup.cs`](Examples/Startup.cs) file. The file is from the [piranha.razor](https://piranhacms.org/docs/master/basics/project-templates) template with relevant code added.

## Code adjustments in your Piranha project
1. Get and add the SpamDetector module to your Piranha project, either by source or package:
	1. Using source code: Downloading the source code and add a project reference to `Zon3.SpamDetector.csproj` _OR_
	1. Using NuGet package manager: Add the SpamDetector package as a NuGet dependency  
1. Add a reference to `Zon3.SpamDetector` in your Piranha project startup file
1. In `Startup.cs` add code to register SpamDetector service and middleware and attach it to the proper hook: 
    1. Register `SpamDetector` service 
	1. Register `SpamDetector` middleware
    1. Register a Comment validation hook and call `SpamDetector.ReviewAsync(Comment c)` to get validation result.
	1. Make sure the hook use the validation to set the comment status (`IsApproved`)
# ConfigurationValues that are mandatory are marked with (Required). Without these properly set, the module won't work.
## API Config
These settings controls the module:

* (Required) Enable SpamDetector module : use this to turn the module on or off.
* (Required) Test mode: use this while setting up and testing the module. This will advice Akismet that comments sent are for testing purposes only. Don't forget to turn off once the module is setup and testing is done.
* (Required) API URL: the personal, site-specific API key to use to make calls to Akismet (see [akismet.com/development/api](https://akismet.com/development/api)).

## Site Config
These settings are sent to Akismet along with the comments to help review the content submitted:

*  (Required) Site URL: The full URL of the frontpage of the blog/site using the SpamDetector module. 
* Site language: The language(s) used by the site (ie the expected languages of the comments submitted). A comma separated list of  [ISO 639-1 formatted](https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes) for all languages used by the site.
* Site encoding: The character encoding for the form values of submitted comments ('Your name', 'your email address', 'An (optional) URL to your website' and 'Your awesome comment').
* User role: The user role of the user who submitted the comment (if any).
# Further readingSelected links relevant to this module:* Piranha Modules: 
	* [piranhacms.org/docs/extensions/modules](https://piranhacms.org/docs/extensions/modules)
	* [github.com/PiranhaCMS/piranha.modules](https://github.com/PiranhaCMS/piranha.modules)
* Piranha Hooks: 
	* [piranhacms.org/docs/application/hooks](https://piranhacms.org/docs/application/hooks)
* Akismet API documentation:
    * [akismet.com/development/api/#detailed-docs](https://akismet.com/development/api/#detailed-docs)
    * [akismet.com/development/api/#comment-check](https://akismet.com/development/api/#comment-check)
* Related Piranha issues:
    * Redesign of Hooks: [github.com/PiranhaCMS/piranha.core/issues/1236](https://github.com/PiranhaCMS/piranha.core/issues/1236)
