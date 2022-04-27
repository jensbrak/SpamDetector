using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Piranha.Manager;
using Piranha.Manager.Models;
using Zon3.SpamDetector.Models;
using Zon3.SpamDetector.Services;


namespace Zon3.SpamDetector.Controllers
{
    /// <summary>
    /// API controller for the SpamDetector config management.
    /// </summary>
    [Area("Manager")]
    [Route("manager/api/spamdetector")]
    [Authorize(Policy = Permission.Admin)]
    [ApiController]
    public class SpamDetectorConfigApiController : Controller
    {
        private readonly SpamDetectorConfigService _config;
        private readonly SpamDetectorLocalizer _localizer;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="config">Config service</param>
        /// <param name="localizer">Localization service</param>
        public SpamDetectorConfigApiController(SpamDetectorConfigService config, SpamDetectorLocalizer localizer)
        {
            _config = config;
            _localizer = localizer;
        }

        /// <summary>
        /// Gets the list model.
        /// </summary>
        /// <returns>The list model</returns>
        [Route("list")]
        [HttpGet]
        [Authorize(Policy = Permission.Config)]
        public SpamDetectorConfigModel List()
        {
            return _config.Get();
        }

        /// <summary>
        /// Save the given model.
        /// </summary>
        /// <param name="configModel">The config model</param>
        /// <returns>The result of the save operation</returns>
        [Route("save")]
        [HttpPost]
        [Authorize(Policy = Permission.ConfigEdit)]
        public AsyncResult Save(SpamDetectorConfigModel configModel)
        {
            try
            {
                _config.Save(configModel);
            }
            catch
            {
                return new AsyncResult
                {
                    Status = new StatusMessage
                    {
                        Type = StatusMessage.Error,
                        Body = _localizer.Config["An error occurred while saving SpamDetector settings"]
                    }
                };
            }
            return new AsyncResult
            {
                Status = new StatusMessage
                {
                    Type = StatusMessage.Success,
                    Body = _localizer.Config["SpamDetector settings was successfully saved"]
                }
            };
        }
    }
}
