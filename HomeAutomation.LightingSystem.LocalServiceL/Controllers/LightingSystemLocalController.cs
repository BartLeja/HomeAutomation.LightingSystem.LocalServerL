using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomeAutomation.LightingSystem.LocalServiceL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LightingSystemLocalController : ControllerBase
    {
        private readonly ILogger<LightingSystemLocalController> _logger;

        public LightingSystemLocalController(ILogger<LightingSystemLocalController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
           return "Lighting System Local -> Works";
        }
    }
}
