namespace Zon3.SpamDetector
{
    /// <summary>
    /// The result of a SpamDetection request. 
    /// </summary>
    public class CommentReview
    {
        public bool Approved { get; set;  }
        public bool? Blacklisted { get; set; } = null;
        public int? SpamScore { get; set; } = null;
        public string Information { get; set; } = null;
    }
}
