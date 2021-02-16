using System;
using System.Collections.Generic;
using System.Text;

namespace Zon3.SpamDetector
{
    /// <summary>
    /// Options for SpamDetector with default values 
    /// </summary>
    public class SpamDetectorOptions
    {
        public const string Identifier = "SpamDetector";
        public bool Enabled { get; set; } = true;
        public string SpamApiUrl { get; set; }
        public string SiteUrl { get; set; } 
        public string SiteLanguage { get; set; } = "en-US";
        public string SiteEncoding { get; set; } = "UTF8";
        public string UserRole { get; set; } = "guest";
        public bool IsTest { get; set; } = true;
    }
}
