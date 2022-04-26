# SpamDetector
| Server | Status |
| ------ | ------ |
| GitHub Actions | [![.NET Linux](https://github.com/jensbrak/SpamDetector/actions/workflows/dotnet.yml/badge.svg)](https://github.com/jensbrak/SpamDetector/actions/workflows/dotnet.yml) |
| GitHub Actions | [![.NET Win](https://github.com/jensbrak/SpamDetector/actions/workflows/dotnet-win.yml/badge.svg)](https://github.com/jensbrak/SpamDetector/actions/workflows/dotnet-win.yml) |
| NuGet | [![.github/workflows/nuget.yml](https://github.com/jensbrak/SpamDetector/actions/workflows/nuget.yml/badge.svg)](https://github.com/jensbrak/SpamDetector/actions/workflows/nuget.yml) |

# Description
A Piranha CMS module providing spam detection for page and post comments.

# About
Project page: [zon3.se/spamdetector](https://zon3.se/spamdetector)<br>
Source code: [github.com/jensbrak/SpamDetector](https://github.com/jensbrak/SpamDetector)<br>
<br>
Piranha CMS provides a feature to allow visitors to comment posts and pages. 
Out of the box, there is no mechanism to prevent spamming the site with unsolicited comments.
This module adds automatic spam detection functionality for submitted comments. 
It does so by using the free to use Akismet service for spam detection.
Once configured, the module will intercept all comments before they are published. 
If considered spam, the comment is marked as not approved, effectively stopping it from being published.
If not considered spam, the comment is marked as approved and is published.

# Note
While providing working antispam functionality, the purpose of this module was primarly to explore and learn about Piranha.
The documentation and samples are helpful when writing a module but without one exception: Manager support.
I wanted to see what it took to make a module with full Manager support, including persistance, localization while keeping the look and feel of the internal Piranha modules. 
In other words: You might have use for this module if you want to understand how Manager support can be added to a custom Piranha module.

# Dependencies
* `Zon3.SpamDetector.Localization`
* `Microsoft.Extensions.Http` version 6
* `Microsoft.Extensions.Localization` version 6
* `Piranha` version 10
* `Piranha.Manager` version 10
* Also: An Akismet API key

# Demo
Please go ahead and try it out by posting a comment with a greeting on my blog. It runs PiranhaCMS with this module active. If your comment is directly visible it's been classified by akismet as non-spam and approved by the SpamDetector module as a valid comment. If not, stop spamming! (Or report a bug to me ;) )

My PiranhaCMS demo site: [zon3.se](https://zon3.se)

# Installation
See Instructions below and/or the example [`Startup.cs`](Examples/Startup.cs) (for Piranha prior version 10.0.3) or [`Program.cs`](Examples/Program.cs) (for Piranha version 10.0.3 and up). The files are from the [piranha.razor](https://piranhacms.org/docs/master/basics/project-templates) template with relevant code added.

## Code adjustments in your Piranha project
1. Get and add the SpamDetector module to your Piranha project, either by source or package:
	1. Using source code: Downloading the source code and add a project reference to `Zon3.SpamDetector.csproj` + `Zon3.SpamDetector.localization.csproj` _OR_
	1. Using NuGet package manager: Add the SpamDetector and SpamDetector.Localization packages as a NuGet dependencies 
1. Add a reference to `Zon3.SpamDetector` in your Piranha project startup file (Program.cs for Piranha version 10.0.3 and up, Startup.cs otherwise)
1. In startup file add code to register SpamDetector service and middleware and attach it to the proper hook: 
    1. Register `SpamDetector` service 
	1. Register `SpamDetector` middleware
    1. Register a Comment validation hook and call `SpamDetector.ReviewAsync(Comment c)` to get validation result.
	1. Make sure the hook use the validation to set the comment status (`IsApproved`)

# Settings
Once installed, the module is accessed and configured via Piranha Manager, under `Settings` in the menu.
Values that are mandatory are marked with (Required). Without these properly set, the module won't work.

## Module settings
These settings control the module:

* (Required) Module enabled : use this to turn the module on or off.
* (Required) Test mode enabled: use this while setting up and testing the module. This will advice Akismet that comments sent are for testing purposes only. Don't forget to turn off once the module is setup and testing is done.
* (Required) API URL: the personal, site-specific API key to use to make calls to Akismet (see [akismet.com/development/api](https://akismet.com/development/api)).

## Site settings
These settings are sent to Akismet along with the comments to help review the content submitted:

* (Required) URL: The full URL of the frontpage of the blog/site using the SpamDetector module. 
* Language: The language(s) used by the site (ie the expected languages of the comments submitted). A comma separated list of  [ISO 639-1 formatted](https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes) for all languages used by the site.
* Encoding: The character encoding for the form values of submitted comments ('Your name', 'your email address', 'An (optional) URL to your website' and 'Your awesome comment').
* User role: The user role of the user who submitted the comment (if any).

# Localization
The module itself supports localization, it works similar to how Piranha is localized. 
Note: the support is not complete: the texts with module info does not yet support localization.

# Issues and feedback
Found an issue with the module? Feedback or ideas about it?<br> 
Visit the GitHub page of the module and submit an issue.<br>
<br>
Issues: [github.com/jensbrak/SpamDetector/issues](https://github.com/jensbrak/SpamDetector/issues)

# Further reading
Selected links relevant to this module:

* Piranha Modules: 
	* [piranhacms.org/docs/extensions/modules](https://piranhacms.org/docs/extensions/modules)
	* [github.com/PiranhaCMS/piranha.modules](https://github.com/PiranhaCMS/piranha.modules)
* Piranha Hooks: 
	* [piranhacms.org/docs/application/hooks](https://piranhacms.org/docs/application/hooks)
* Akismet API documentation:
    * [akismet.com/development/api/#detailed-docs](https://akismet.com/development/api/#detailed-docs)
    * [akismet.com/development/api/#comment-check](https://akismet.com/development/api/#comment-check)
* Related Piranha issues:
    * Redesign of Hooks: [github.com/PiranhaCMS/piranha.core/issues/1236](https://github.com/PiranhaCMS/piranha.core/issues/1236)

