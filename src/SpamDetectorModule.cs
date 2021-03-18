using Piranha;
using Piranha.Extend;
using Piranha.Manager;
using System;

namespace Zon3.SpamDetector
{
    /// <summary>
    /// Piranha Module definition for SpamDetector.
    /// </summary>
    public class SpamDetectorModule : IModule
    {
        /// <summary>
        /// Gets the Author.
        /// </summary>
        public string Author => "Jens Bråkenhielm";

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public string Name => "Zon3.SpamDetector";

        /// <summary>
        /// Gets the version.
        /// </summary>
        public string Version => Utils.GetAssemblyVersion(GetType().Assembly);

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description => "Piranha Module detecting comment spam using Akismet";

        /// <summary>
        /// Gets the package url.
        /// </summary>
        public string PackageUrl => "https://www.nuget.org/packages/Zon3.SpamDetector";

        /// <summary>
        /// Gets the icon url.
        /// </summary>
        public string IconUrl => "https://zon3.se/uploads/35e58d2f-bc69-4fd9-8dd1-49c4ad56e53b/PhiddleLogo_512x512.png";

        /// <summary>
        /// Initializes the module.
        /// </summary>
        public void Init()
        {
            // We prefer to reside under Settings, at the bottom
            var settingsIndex = Menu.Items.FindLastIndex(menuItem => menuItem.Name.Equals("Settings", StringComparison.OrdinalIgnoreCase));

            Menu.Items[settingsIndex].Items.Add(new MenuItem
            {
                InternalId = "SpamDetector",
                Name = "SpamDetector",
                Route = "~/manager/spamdetector",
                Css = "fas fa-comment-slash",
            });
        }
    }
}
