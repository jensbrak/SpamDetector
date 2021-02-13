using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Piranha;
using Piranha.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Zon3.SpamDetector
{
    public class AkismetSpamDetector : SpamDetector
    {
        public AkismetSpamDetector(IApi piranhaApi, IOptions<SpamDetectorOptions> options, IHttpClientFactory clientFactory, ILoggerFactory logger) : base(piranhaApi, options, clientFactory, logger)
        {
            // Intentionally left empty
        }
        
        protected override async Task<HttpRequestMessage> GetSpamRequestMessageAsync(Comment comment)
        {
            _commentId = comment.Id;

            var post = await _piranha.Posts.GetByIdAsync(comment.ContentId);
            var page = await _piranha.Pages.GetByIdAsync(post != null ? post.BlogId : comment.ContentId);
            var permalink = post != null ? post.Permalink : page.Permalink;
            var permalinkFull = $"{_options.SiteUrl}{permalink}";

            var parameters = new Dictionary<string, string>
                {
                    {"blog", HttpUtility.UrlEncode(_options.SiteUrl)},
                    {"user_ip", HttpUtility.UrlEncode(comment.IpAddress)},
                    {"user_agent", HttpUtility.UrlEncode(comment.UserAgent)},
                    {"referrer", HttpUtility.UrlEncode(string.Empty)}, // ???
                    {"permalink", HttpUtility.UrlEncode(permalinkFull)},
                    {"comment_type", HttpUtility.UrlEncode("comment")},
                    {"comment_author", HttpUtility.UrlEncode(comment.Author ?? string.Empty)},
                    {"comment_author_email", HttpUtility.UrlEncode(comment.Email ?? string.Empty)},
                    {"comment_author_url", HttpUtility.UrlEncode(comment.Url ?? string.Empty)},
                    {"comment_content", HttpUtility.UrlEncode(comment.Body ?? string.Empty)},
                    {"comment_date_gmt", HttpUtility.UrlEncode(comment.Created.ToString())},
                    {"comment_post_modified_gmt", HttpUtility.UrlEncode(comment.Created.ToString())},
                    {"blog_lang", HttpUtility.UrlEncode(_options.SiteLanguage)},
                    {"blog_charset", HttpUtility.UrlEncode(_options.SiteEncoding)},
                    {"user_role", HttpUtility.UrlEncode(_options.UserRole)},
                    {"is_test", HttpUtility.UrlEncode(_options.IsTest.ToString())}
                };

            _logger.LogDebug(
                "Sending Spam API request for comment '{1}' from IP '{2}'...",
                _commentId,
                comment.IpAddress);

            return new HttpRequestMessage(HttpMethod.Post, _options.SpamApiUrl)
            {
                Content = new FormUrlEncodedContent(parameters)
            };
        }

        protected override async Task<CommentReview> GetCommentReviewFromResponse(HttpResponseMessage response)
        {
            var answer = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

            _logger.LogDebug(
                "Received Spam API response for comment '{1}' = \"{2}\"",
                _commentId,
                answer);

            return new CommentReview()
            {
                // Akismet returns 'true' if it's spam, 'false' otherwise, so the comment is approved if it's NOT spam
                Approved = answer.Equals("false", StringComparison.OrdinalIgnoreCase),
                Information = response.Headers.ToString()
            };
        }
    }
}