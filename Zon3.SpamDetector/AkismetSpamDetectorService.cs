using Microsoft.Extensions.Logging;
using Piranha;
using Piranha.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Zon3.SpamDetector.Services;

namespace Zon3.SpamDetector
{
    public class AkismetSpamDetectorService : SpamDetectorService
    {
        /// <inheritdoc cref="SpamDetectorService" select="param"/>
        /// <summary>
        /// The Akismet anti-spam service implementation of the <see cref="SpamDetectorService"/> core functionality.
        /// Composes a request to Akismet with as much relevant information as possible provided and
        /// translates a response from Akismet to a review result in form of a <see cref="CommentReview"/>.
        /// </summary>
        public AkismetSpamDetectorService(IApi piranha, SpamDetectorConfigService config, IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory) : base(piranha, config, httpClientFactory, loggerFactory)
        {
            // Intentionally left empty
        }
        
        /// <inheritdoc cref="SpamDetectorService"/>
        protected override async Task<HttpRequestMessage> GetSpamRequestMessageAsync(Comment comment)
        {
            CommentId = comment.Id;

            var post = await Piranha.Posts.GetByIdAsync(comment.ContentId);
            var page = await Piranha.Pages.GetByIdAsync(post?.BlogId ?? comment.ContentId);
            var permalink = post != null ? post.Permalink : page.Permalink;
            var permalinkFull = $"{ConfigModel.SiteUrl}{permalink}";

            var parameters = new Dictionary<string, string>
                {
                    {"blog", HttpUtility.UrlEncode(ConfigModel.SiteUrl)},
                    {"user_ip", HttpUtility.UrlEncode(comment.IpAddress)},
                    {"user_agent", HttpUtility.UrlEncode(comment.UserAgent)},
                    {"referrer", HttpUtility.UrlEncode(string.Empty)}, // ???
                    {"permalink", HttpUtility.UrlEncode(permalinkFull)},
                    {"comment_type", HttpUtility.UrlEncode("comment")},
                    {"comment_author", HttpUtility.UrlEncode(comment.Author ?? string.Empty)},
                    {"comment_author_email", HttpUtility.UrlEncode(comment.Email ?? string.Empty)},
                    {"comment_author_url", HttpUtility.UrlEncode(comment.Url ?? string.Empty)},
                    {"comment_content", HttpUtility.UrlEncode(comment.Body ?? string.Empty)},
                    {"comment_date_gmt", HttpUtility.UrlEncode(comment.Created.ToUniversalTime().ToString("R"))}, // RFC1123
                    {"comment_post_modified_gmt", HttpUtility.UrlEncode(comment.Created.ToString(CultureInfo.InvariantCulture))},
                    {"blog_lang", HttpUtility.UrlEncode(ConfigModel.SiteLanguage)},
                    {"blog_charset", HttpUtility.UrlEncode(ConfigModel.SiteEncoding)},
                    {"user_role", HttpUtility.UrlEncode(ConfigModel.UserRole)},
                    {"is_test", HttpUtility.UrlEncode(ConfigModel.IsTest.ToString())}
                };

            return new HttpRequestMessage(HttpMethod.Post, ConfigModel.SpamApiUrl)
            {
                Content = new FormUrlEncodedContent(parameters)
            };
        }

        /// <inheritdoc cref="SpamDetectorService"/>
        protected override async Task<CommentReview> GetCommentReviewFromResponse(HttpResponseMessage response)
        {
            var answer = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

            Logger.LogDebug("Received Spam API response for comment '{1}' = \"{2}\"", CommentId, answer);

            return new CommentReview()
            {
                // Akismet returns 'true' if it's spam, 'false' otherwise, so the comment is approved if it's NOT spam
                Approved = answer.Equals("false", StringComparison.OrdinalIgnoreCase),
                Information = response.Headers.ToString()
            };
        }
    }
}