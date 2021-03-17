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
    /// Designed for https://akismet.com/
    /// 
    /// Briefly considered anti-spam SaaS:
    ///     https://cleantalk.org
    ///     https://www.oopspam.com/
    /// </summary>
    public abstract class SpamDetector : ISpamDetector
    {
        protected IApi _piranha;

        protected ILogger _logger;

        protected IHttpClientFactory _httpClientFactory;

        protected SpamDetectorConfigEditModel _configEditModel;

        protected Guid _commentId;

        public bool Enabled => _configEditModel.Enabled;

        public SpamDetector(IApi piranhaApi, SpamDetectorConfigService configService, IHttpClientFactory clientFactory, ILoggerFactory logger)
        {
            _piranha = piranhaApi;
            _httpClientFactory = clientFactory;
            _configEditModel = configService.Get();

            if (logger != null)
            {
                _logger = logger.CreateLogger(this.GetType().FullName);
            }

            if (string.IsNullOrEmpty(_configEditModel.SpamApiUrl))
            {
                var msg = $"Option SpamApiUrl is missing: value mandatory";
                _logger.LogError(msg);
                throw new InvalidOperationException(msg);
            }

            if (string.IsNullOrEmpty(_configEditModel.SiteUrl))
            {
                _logger.LogWarning("Option SiteUrl is missing: results may be wrong");
            }

            if (_configEditModel.IsTest)
            {
                _logger.LogWarning("Option IsTest is true: no live requests will be made");
            }
        }

        public async Task<CommentReview> ReviewAsync(Comment comment)
        {
            _commentId = comment.Id;

            // If not enabled, warn and use existing value for comment  
            if (!Enabled)
            {
                _logger.LogWarning($"Option Enabled is false: no review for comment '{_commentId}' made");

                return new CommentReview() { Approved = comment.IsApproved };
            }

            // Get a client for API request
            var client = _httpClientFactory.CreateClient();

            // Create a request with relevant parameters added from comment
            var requestMessage = await GetSpamRequestMessageAsync(comment);

            // Send request and make sure we have a valid response
            var response = await client.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            // Get review result from response
            var review = await GetCommentReviewFromResponse(response);

            // Review done
            return review;
        }

        // Leave implementation to derived class
        protected abstract Task<HttpRequestMessage> GetSpamRequestMessageAsync(Comment comment);

        // Leave implementation to derived class
        protected abstract Task<CommentReview> GetCommentReviewFromResponse(HttpResponseMessage response);
    }
}
