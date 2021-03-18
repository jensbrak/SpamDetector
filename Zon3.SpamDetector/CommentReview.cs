namespace Zon3.SpamDetector
{
    /// <summary>
    /// The result of a review of a comment made by an anti-spam service.
    /// How it is used is up to the API-specific implementation of the <see cref="SpamDetectorService"/> service.
    /// How the result should be interpreted is up to the caller of <see cref="SpamDetectorService.ReviewAsync"/>.
    /// </summary>
    public class CommentReview
    {
        /// <summary>
        /// If the comment is approved, ie NOT considered spam.
        /// </summary>
        public bool Approved { get; set;  }
        /// <summary>
        /// Optional information about the comment review made.
        /// </summary>
        public string Information { get; set; } = null;
    }
}
