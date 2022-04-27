using Microsoft.Extensions.Localization;

namespace Zon3.SpamDetector
{
    /// <summary>
    /// Localization of SpamDetector.
    /// </summary>
    public class SpamDetectorLocalizer
    {
        /// <summary>
        /// Gets the config related localizer.
        /// </summary>
        public IStringLocalizer<Localization.Config> Config { get; private set; }
        /// <summary>
        /// Gets the info related localizer.
        /// </summary>
        public IStringLocalizer<Localization.Info> Info { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="config">Config localizer</param>
        /// <param name="info">Info localizer</param>
        public SpamDetectorLocalizer(
            IStringLocalizer<Localization.Config> config,
            IStringLocalizer<Localization.Info> info)
        {
            Config = config;
            Info = info;
        }
    }
}
