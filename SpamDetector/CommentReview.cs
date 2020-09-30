using System;
using System.Collections.Generic;
using System.Text;

namespace Zon3.SpamDetector
{
    public class CommentReview
    {
        public bool IsSpam { get; set;  }
        public bool Blacklisted { get; set; }

        public int? SpamScore { get; set; } = null;

        public string Information { get; set; } = string.Empty;
    }
}
