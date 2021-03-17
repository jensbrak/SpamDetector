using Microsoft.Extensions.Localization;

namespace Zon3.SpamDetector.Localization
{
    public class SpamDetectorLocalizer 
    {
        public IStringLocalizer<Config> Config { get; private set; }
        public IStringLocalizer<Info> Info { get; private set; }

        public SpamDetectorLocalizer(
            IStringLocalizer<Config> config, 
            IStringLocalizer<Info> info)
        {
            Config = config;
            Info = info;
        }
    }
}
