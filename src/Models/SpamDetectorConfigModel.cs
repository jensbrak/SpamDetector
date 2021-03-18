namespace Zon3.SpamDetector.Models
{
    /// <summary>
    /// The SpamDetector config model.
    /// </summary>
    public class SpamDetectorConfigModel
    {
        /// <summary>
        /// If SpamDetector module is active.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// If requests to anti-spam API should be marked as test.
        /// </summary>
        public bool IsTest { get; set; }
        /// <summary>
        /// The anti-spam API URL to use for requests.
        /// </summary>
        public string SpamApiUrl { get; set; }
        /// <summary>
        /// The URL to the site sending requests.
        /// </summary>
        public string SiteUrl { get; set; }
        /// <summary>
        /// The language(s) used by the site sending requests. 
        /// </summary>
        public string SiteLanguage { get; set; }
        /// <summary>
        /// The encoding of the comment form values.
        /// </summary>
        public string SiteEncoding { get; set; }
        /// <summary>
        /// The user role of the users submitting comments.
        /// </summary>
        public string UserRole { get; set; }
    }
}
