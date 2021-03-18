using Microsoft.Extensions.Localization;

namespace Zon3.SpamDetector.Localization
{
    /// <summary>
    /// Localization of SpamDetector.
    /// </summary>
    public class SpamDetectorLocalizer 
    {
        /// <summary>
        /// Gets the config related localizer.
        /// </summary>
        public IStringLocalizer<Config> Config { get; }
        /// <summary>
        /// Gets the info related localizer.
        /// </summary>
        public IStringLocalizer<Info> Info { get; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="config">Config localizer</param>
        /// <param name="info">Info localizer</param>
        public SpamDetectorLocalizer(
            IStringLocalizer<Config> config, 
            IStringLocalizer<Info> info)
        {
            Config = config;
            Info = info;
        }
    }
}
