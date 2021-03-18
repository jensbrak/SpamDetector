using Microsoft.Extensions.Logging;
using Piranha;
using Piranha.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Zon3.SpamDetector.Models;
using Zon3.SpamDetector.Services;

namespace Zon3.SpamDetector
{
    /// <summary>
    /// Base class for the core functionality of the SpamDetector module.
    /// While generic and intended to be overridden, it is designed for (and using)
    /// the Akismet anti-spam service: https://akismet.com/ 
    /// 
    /// Briefly considered anti-spam SaaS:
    ///     https://cleantalk.org
    ///     https://www.oopspam.com/
    /// </summary>
    public abstract class SpamDetectorService : ISpamDetectorService
    {
        protected IApi Piranha;
        protected ILogger Logger;
        protected IHttpClientFactory HttpClientFactory;
        protected SpamDetectorConfigModel ConfigModel;
        protected Guid CommentId;

        public bool Enabled => ConfigModel.Enabled;

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="piranha">The Piranha API</param>
        /// <param name="config">The configuration to use</param>
        /// <param name="httpClientFactory">The factory to use to get http client for requests</param>
        /// <param name="loggerFactory">The factory to use to get a logger</param>
        protected SpamDetectorService(IApi piranha, SpamDetectorConfigService config, IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
        {
            Piranha = piranha;
            HttpClientFactory = httpClientFactory;
            ConfigModel = config.Get();

            if (loggerFactory != null)
            {
                Logger = loggerFactory.CreateLogger(this.GetType().FullName);
            }

            if (!ConfigModel.Enabled)
            {
                Logger.LogWarning("Module disabled by configuration: comments will not be reviewed/changed");
            }

            if (string.IsNullOrEmpty(ConfigModel.SpamApiUrl))
            {
                const string msg = "Module not configured properly, mandatory value missing: SpamApiUrl";
                Logger.LogError(msg);
                throw new InvalidOperationException(msg);
            }

            if (string.IsNullOrEmpty(ConfigModel.SiteUrl))
            {
                Logger.LogWarning("Option SiteUrl is missing: results may be wrong");
            }

            if (ConfigModel.IsTest)
            {
                Logger.LogWarning("Option IsTest is true: no live requests will be made");
            }
        }

        /// <summary>
        /// Review a comment by sending relevant information to the anti-spam service.
        /// </summary>
        /// <param name="comment">Comment to send for review</param>
        /// <returns>Interpreted and selected information about the result of the review</returns>
        public async Task<CommentReview> ReviewAsync(Comment comment)
        {
            CommentId = comment.Id;

            if (!Enabled)
            {
                return new CommentReview() { Approved = comment.IsApproved };
            }

            Logger.LogDebug($"Composing API request for comment '{CommentId}' by '{comment.Email}'");
            var requestMessage = await GetSpamRequestMessageAsync(comment);
            Logger.LogDebug("Sending API request for comment '{1}' from IP '{2}'...", CommentId, comment.IpAddress);
            var response = await HttpClientFactory.CreateClient().SendAsync(requestMessage);
            Logger.LogDebug("Received API response for comment '{1}' = '{2}'", CommentId, response.Content);
            response.EnsureSuccessStatusCode();
            Logger.LogDebug("Interpreting API response for comment '{1}' = '{2}'", CommentId, requestMessage);
            var review = await GetCommentReviewFromResponse(response);

            return review;
        }

        /// <summary>
        /// The API specific implementation that compose a request message to the anti-spam service.
        /// It should provide enough information to the service to enable it to review the comment
        /// and determine if it is to be considered spam or not.
        /// </summary>
        /// <param name="comment">Comment to send for review</param>
        /// <returns>A request ready to be sent to the anti-spam API</returns>
        protected abstract Task<HttpRequestMessage> GetSpamRequestMessageAsync(Comment comment);

        /// <summary>
        /// The API specific implementation that interprets the request response from the anti-spam
        /// service. It should translate the response returning a usable <see cref="CommentReview"/>.
        /// </summary>
        /// <param name="response"></param>
        /// <returns>Information about the result of the review</returns>
        protected abstract Task<CommentReview> GetCommentReviewFromResponse(HttpResponseMessage response);
    }
}
