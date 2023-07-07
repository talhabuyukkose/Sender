using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sender.Core.Models;
using Sender.Infrastructure.Services.Tsoft;

namespace Sender.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly TsoftClient tsoftClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, TsoftClient tsoftClient)
        {
            _logger = logger;
            this.tsoftClient = tsoftClient;
        }

        [HttpPost(Name = "GetWeatherForecast")]
        [RequestSizeLimit(1 * 1024 * 1024)]
        //[RequestFormLimits(BufferBody = true, MultipartBodyLengthLimit = 10485760000)]
        public async Task<object> Get([FromForm] SiteUser siteUser)
        {
            //SiteUser siteUser = new SiteUser()
            //{
            //    BaseUrl = "https://arge-talhabuyukkose.1isim.com",
            //    UserName = "talha",
            //    Password = "Talha.01"
            //};
            var responses = new List<object>();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                foreach (var formFile in siteUser.FromFiles)
                {
                    formFile.CopyTo(memoryStream);

                    var byteFile = memoryStream.ToArray();

                    var response = await tsoftClient.SendImage(
                        siteUser,
                        byteFile,
                        formFile.FileName,
                        formFile.FileName.Replace(".jpg", "").Replace(".jpeg", "")
                        );
                    responses.Add(response);

                    memoryStream.Position = 0;
                }

                return responses;
            }
        }
    }
}