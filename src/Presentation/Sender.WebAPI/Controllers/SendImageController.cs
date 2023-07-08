using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sender.Core.Models;
using Sender.Infrastructure.Services.Tsoft;

namespace Sender.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendImageController : ControllerBase
    {
        private readonly ILogger<SendImageController> logger;
        private readonly TsoftClient tsoftClient;

        public SendImageController(ILogger<SendImageController> logger, TsoftClient tsoftClient)
        {
            this.logger = logger;
            this.tsoftClient = tsoftClient;
        }

        //    BaseUrl = "https://arge-talhabuyukkose.1isim.com",
        //    UserName = "talha",
        //    Password = "Talha.01"

        [HttpPost]
        [RequestSizeLimit(1 * 1024 * 1024)]
        //[RequestFormLimits(BufferBody = true, MultipartBodyLengthLimit = 10485760000)]
        public async Task<object> Get([FromForm] SiteUser siteUser)
        {

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
