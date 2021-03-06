using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zon3.SpamDetector.Models
{
    public class SpamDetectorConfigEditModel
    {
        public bool Enabled { get; set; } = true;
        public bool IsTest { get; set; } = true;
        public string SpamApiUrl { get; set; }
        public string SiteUrl { get; set; }
        public string SiteLanguage { get; set; } = "en-US";
        public string SiteEncoding { get; set; } = "UTF8";
        public string UserRole { get; set; } = "guest";
    }
}
