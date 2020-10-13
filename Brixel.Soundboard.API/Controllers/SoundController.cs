using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Brixel.Soundboard.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Brixel.Soundboard.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SoundController : ControllerBase
    {
        private readonly ILogger<SoundController> _logger;
        private readonly IMqttClientService _mqttClientService;

        public SoundController(ILogger<SoundController> logger, IMqttClientService mqttClientService)
        {
            _logger = logger;
            _mqttClientService = mqttClientService;
        }

        [HttpPost("play")]
        public async Task Play([FromForm]IFormFileCollection formFileCollection)
        {
            var singleFile = formFileCollection.Single();
            var memoryStream = new MemoryStream();
            await using (memoryStream)
            {
                await singleFile.CopyToAsync(memoryStream);
            }

            await _mqttClientService.Publish(memoryStream);
        }

    }
}