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
        private readonly string _piranhaVersion;
        private readonly string _pluginVersion;

        /// <inheritdoc cref="SpamDetectorService" select="param"/>
        /// <summary>
        /// The Akismet anti-spam service implementation of the <see cref="SpamDetectorService"/> core functionality.
        /// Composes a request to Akismet with as much relevant information as possible provided and
        /// translates a response from Akismet to a review result in form of a <see cref="CommentReview"/>.
        /// </summary>
        public AkismetSpamDetectorService(IApi api, SpamDetectorConfigService config, IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory) : base(api, config, httpClientFactory, loggerFactory)
        {
            _piranhaVersion = Utils.GetAssemblyVersion(typeof(App).Assembly);
            _pluginVersion = Utils.GetAssemblyVersion(typeof(SpamDetectorModule).Assembly);
        }
        
        /// <inheritdoc cref="SpamDetectorService"/>
        protected override async Task<HttpRequestMessage> GetSpamRequestMessageAsync(Comment comment)
        {
            var userAgent = $"Piranha CMS/{_piranhaVersion} | SpamDetector/{_pluginVersion}";

            string permalink;
            var post = await Api.Posts.GetByIdAsync(comment.ContentId);
            if (post != null)
            {
                permalink = $"{ConfigModel.SiteUrl}{post.Permalink}";
            }
            else
            {
                var page = await Api.Pages.GetByIdAsync(post?.BlogId ?? comment.ContentId);
                permalink = $"{ConfigModel.SiteUrl}{page.Permalink}";
            }

            var parameters = new Dictionary<string, string>
                {
                    {"blog", HttpUtility.UrlEncode(ConfigModel.SiteUrl)},
                    {"user_ip", HttpUtility.UrlEncode(comment.IpAddress)},
                    {"user_agent", HttpUtility.UrlEncode(userAgent)}, // Ignore comment.UserAgent and use Akismet preferred format
                    {"referrer", HttpUtility.UrlEncode(string.Empty)}, // ???
                    {"permalink", HttpUtility.UrlEncode(permalink)},
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
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            
            // If answer is neither true or false, the response is unexpected and something's wrong. 
            // It may be a bad URL for the API call, either way, report it
            var answerIsFalse = responseText.Equals("false", StringComparison.OrdinalIgnoreCase);
            var answerIsTrue = responseText.Equals("true", StringComparison.OrdinalIgnoreCase);

            if (!answerIsFalse && !answerIsTrue)
            {
                Logger.LogError($"Got unexpected Spam API response: {responseText}");
                return null;
            }

            return new CommentReview()
            {
                // Akismet returns 'true' if it's spam, 'false' otherwise.
                // Comment approved if Akismet returns false. in other words.
                Approved = answerIsFalse,
                Information = response.Headers.ToString()
            };
        }
    }
}