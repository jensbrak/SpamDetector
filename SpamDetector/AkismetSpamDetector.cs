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
        public AkismetSpamDetector(IApi piranhaApi, IHttpClientFactory clientFactory, IOptions<SpamDetectorOptions> options) : base(piranhaApi, clientFactory, options)
        {
            // Intentionally left empty
        }
        
        protected override async Task<HttpRequestMessage> GetSpamRequestMessageAsync(Comment comment)
        {
            var post = await PiranhaApi.Posts.GetByIdAsync(comment.ContentId);
            var page = await PiranhaApi.Pages.GetByIdAsync(post != null ? post.BlogId : comment.ContentId);
            var permalink = post != null ? post.Permalink : page.Permalink;

            var parameters = new Dictionary<string, string>
                {
                    {"blog", HttpUtility.UrlEncode(Options.SiteUrl)},
                    {"user_ip", HttpUtility.UrlEncode(comment.IpAddress)},
                    {"user_agent", HttpUtility.UrlEncode(comment.UserAgent)},
                    {"referrer", HttpUtility.UrlEncode(string.Empty)},
                    {"permalink", HttpUtility.UrlEncode($"{Options.SiteUrl}{permalink}")},
                    {"comment_type", HttpUtility.UrlEncode("comment")},
                    {"comment_author", HttpUtility.UrlEncode(comment.Author ?? string.Empty)},
                    {"comment_author_email", HttpUtility.UrlEncode(comment.Email ?? string.Empty)},
                    {"comment_author_url", HttpUtility.UrlEncode(comment.Url ?? string.Empty)},
                    {"comment_content", HttpUtility.UrlEncode(comment.Body ?? string.Empty)},
                    {"comment_date_gmt", HttpUtility.UrlEncode(comment.Created.ToString())},
                    {"comment_post_modified_gmt", HttpUtility.UrlEncode(comment.Created.ToString())},
                    {"blog_lang", HttpUtility.UrlEncode(Options.SiteLanguage)},
                    {"blog_charset", HttpUtility.UrlEncode(Options.SiteEncoding)},
                    {"user_role", HttpUtility.UrlEncode(Options.UserRole)},
                    {"is_test", HttpUtility.UrlEncode(Options.IsTest.ToString())}
                };

            return new HttpRequestMessage(HttpMethod.Post, Options.SpamApiUrl)
            {
                Content = new FormUrlEncodedContent(parameters)
            };
        }

        protected override async Task<CommentReview> GetCommentReviewFromResponse(HttpResponseMessage response)
        {
            var answer = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

            return new CommentReview()
            {
                IsSpam = answer.Equals("true", StringComparison.OrdinalIgnoreCase),
                Information = response.Headers.ToString()
            };
        }
    }
}