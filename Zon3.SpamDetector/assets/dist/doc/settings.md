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

