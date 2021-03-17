using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piranha.Manager;
using Piranha.Manager.Models;
using Zon3.SpamDetector.Localization;
using Zon3.SpamDetector.Models;
using Zon3.SpamDetector.Services;


namespace Zon3.SpamDetector.Controllers
{
    [Area("Manager")]
    [Route("manager/api/spamdetector")]
    [Authorize(Policy = Permission.Admin)]
    [ApiController]
    public class SpamDetectorConfigApiController : Controller
    {
        private readonly SpamDetectorConfigService _configService;
        private readonly SpamDetectorLocalizer _localizer;

        public SpamDetectorConfigApiController(SpamDetectorConfigService configService, SpamDetectorLocalizer localizer)
        {
            _configService = configService;
            _localizer = localizer;
        }

        [Route("list")]
        [HttpGet]
        [Authorize(Policy = Permission.Config)]
        public SpamDetectorConfigEditModel List()
        {
            return _configService.Get();
        }

        [Route("save")]
        [HttpPost]
        [Authorize(Policy = Permission.ConfigEdit)]
        public AsyncResult Save(SpamDetectorConfigEditModel configEditModel)
        {
            try
            {
                _configService.Save(configEditModel);
            }
            catch
            {
                return new AsyncResult
                {
                    Status = new StatusMessage
                    {
                        Type = StatusMessage.Error,
                        Body = _localizer.Config["An error occurred while saving SpamDetector config"]
                    }
                };
            }
            return new AsyncResult
            {
                Status = new StatusMessage
                {
                    Type = StatusMessage.Success,
                    Body = _localizer.Config["SpamDetector config was successfully saved"]
                }
            };
        }
    }
}
