using Piranha;
using Zon3.SpamDetector.Models;

namespace Zon3.SpamDetector.Services
{
    /// <summary>
    /// Service for the SpamDetectorService config management.
    /// </summary>
    public class SpamDetectorConfigService
    {
        private readonly IApi _api;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="api">The Piranha API</param>
        public SpamDetectorConfigService(IApi api)
        {
            _api = api;
        }

        /// <summary>
        /// Gets the SpamDetectorService config model.
        /// </summary>
        /// <returns>The SpamDetectorService model</returns>
        public SpamDetectorConfigModel Get()
        {
            using var config = new SpamDetectorConfig(_api);
            return new SpamDetectorConfigModel
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

        /// <summary>
        /// Saves the given SpamDetectorService model to the database.
        /// </summary>
        /// <param name="configModel">The SpamDetectorService model</param>
        public void Save(SpamDetectorConfigModel configModel)
        {
            using var config = new SpamDetectorConfig(_api)
            {
                Enabled = configModel.Enabled,
                IsTest = configModel.IsTest,
                SiteEncoding = configModel.SiteEncoding,
                SiteLanguage = configModel.SiteLanguage,
                SpamApiUrl = configModel.SpamApiUrl,
                UserRole = configModel.UserRole,
                SiteUrl = configModel.SiteUrl
            };
        }
    }
}
