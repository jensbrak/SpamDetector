using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Piranha.Manager;
using Piranha.Manager.Models;
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

        public SpamDetectorConfigApiController(SpamDetectorConfigService configService)
        {
            _configService = configService;
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
                        Body = "An error occurred while saving"
                    }
                };
            }
            return new AsyncResult
            {
                Status = new StatusMessage
                {
                    Type = StatusMessage.Success,
                    Body = "The config was successfully saved"
                }
            };
        }
    }
}
