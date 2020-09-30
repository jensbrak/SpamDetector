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
        public IApi PiranhaApi { get; }

        public IHttpClientFactory ClientFactory { get; }

        public SpamDetectorOptions Options { get; }

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

            // Interpet answer
            var answer = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var isSpam = answer.Equals("true", StringComparison.OrdinalIgnoreCase);

            return new CommentReview() 
            { 
                IsSpam = isSpam,
                Information = response.Headers.ToString()
            };
        }

        // Leave implementation to derived class
        protected abstract Task<HttpRequestMessage> GetSpamRequestMessageAsync(Comment comment);
    }
}
