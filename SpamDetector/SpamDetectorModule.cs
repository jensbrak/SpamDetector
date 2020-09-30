using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Piranha.Extend;

namespace Zon3.SpamDetector
{
    /// <summary>
    /// Comment Moderator Module.
    /// </summary>
    public class SpamDetectorModule : IModule
    {
        /// <summary>
        /// Gets the Author
        /// </summary>
        public string Author => "Zon3";

        /// <summary>
        /// Gets the Name
        /// </summary>
        public string Name => "Zon3.Modules.SpamDetector";

        /// <summary>
        /// Gets the Version
        /// </summary>
        public string Version => Piranha.Utils.GetAssemblyVersion(GetType().Assembly);

        /// <summary>
        /// Gets the description
        /// </summary>
        public string Description => "Module for spam detection in Comments";

        /// <summary>
        /// Gets the package url.
        /// </summary>
        public string PackageUrl => "<not published>";

        /// <summary>
        /// Gets the icon url.
        /// </summary>
        public string IconUrl => "https://geek.zon3.se/uploads/35e58d2f-bc69-4fd9-8dd1-49c4ad56e53b/PhiddleLogo_512x512.png";

        /// <summary>
        /// Initializes the module.
        /// </summary>
        public void Init()
        {
        }
    }
}
