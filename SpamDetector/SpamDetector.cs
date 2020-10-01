using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Piranha;
using Piranha.Models;
using Piranha.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
        protected IApi PiranhaApi { get; }

        protected IHttpClientFactory ClientFactory { get; }

        protected SpamDetectorOptions Options { get; }

        public SpamDetector(IApi piranhaApi, IHttpClientFactory clientFactory, IOptions<SpamDetectorOptions> options) 
        {
            PiranhaApi = piranhaApi;
            ClientFactory = clientFactory;
            Options = options.Value;
        }

        public async Task<CommentReview> ReviewAsync(Comment comment)
        {
            // Get a client for API request
            var client = ClientFactory.CreateClient();

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
