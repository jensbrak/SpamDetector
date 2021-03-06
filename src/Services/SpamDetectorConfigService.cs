using Piranha;
using Zon3.SpamDetector.Models;

namespace Zon3.SpamDetector.Services
{
    public class SpamDetectorConfigService
    {
        private readonly IApi _api;

        public SpamDetectorConfigService(IApi api)
        {
            _api = api;
        }

        public SpamDetectorConfigEditModel Get()
        {
            using var config = new SpamDetectorConfig(_api);
            return new SpamDetectorConfigEditModel
            {
                Enabled = config.Enabled,
                IsTest = config.IsTest,
                SiteEncoding = config.SiteEncoding,
                SiteLanguage = config.SiteLanguage,
                SpamApiUrl = config.SpamApiUrl,
                UserRole = config.UserRole,
                SiteUrl = config.SiteUrl
            };
        }

        public void Save(SpamDetectorConfigEditModel configEditModel)
        {
            using var config = new SpamDetectorConfig(_api)
            {
                Enabled = configEditModel.Enabled,
                IsTest = configEditModel.IsTest,
                SiteEncoding = configEditModel.SiteEncoding,
                SiteLanguage = configEditModel.SiteLanguage,
                SpamApiUrl = configEditModel.SpamApiUrl,
                UserRole = configEditModel.UserRole,
                SiteUrl = configEditModel.SiteUrl
            };
        }
    }
}
