using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sender.Core.Extensions.JsonProcess;
using Sender.Core.Models.ApiModels;
using Sender.Infrastructure.Services.Tsoft;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

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
        public async Task<object> Post([FromForm] SiteUserWithfile siteUser)
        {
            var siteuserdto = new SiteUser()
            {
                BaseUrl = siteUser.BaseUrl,
                Password = siteUser.Password,
                UserName = siteUser.UserName,
            };

            var responses = new List<object>();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                foreach (var formFile in siteUser.FromFiles)
                {
                    formFile.CopyTo(memoryStream);

                    var byteFile = memoryStream.ToArray();

                    var response = await tsoftClient.SendImage(
                        siteuserdto,
                        byteFile,
                        formFile.FileName.Replace(".jpg", "").Replace(".jpeg", "")
                        );
                    responses.Add(response);

                    logger.LogInformation($"{formFile.FileName} jpg dosyası yüklendi.");

                    memoryStream.Position = 0;
                }

                return responses;
            }
        }

        [HttpPost]
        [Route("fromlink")]
        [RequestSizeLimit(1 * 1024 * 1024)]
        //[RequestFormLimits(BufferBody = true, MultipartBodyLengthLimit = 10485760000)]
        public async Task<IActionResult> FromLink([FromQuery] SiteUser siteUser, [FromBody] object content, CancellationToken cancellationToken)
        {
            var siteuserdto = new SiteUser()
            {
                BaseUrl = siteUser.BaseUrl,
                Password = siteUser.Password,
                UserName = siteUser.UserName,
            };

            var imagelist = content.JsonDeserialize<List<SendImageFromUrl>>();

            var parallelOptions = new ParallelOptions
            {
                CancellationToken = cancellationToken,
                MaxDegreeOfParallelism = 4
            };
            ICollection<SendImageFromUrl> imagelistResponse = new List<SendImageFromUrl>();

            await Parallel.ForEachAsync(imagelist, parallelOptions, async(image, cancellationToken) =>
            {
                var response = await tsoftClient.SendImageFromUrl(siteuserdto, image);

               imagelistResponse.Add(response);
            });

            return Ok(imagelistResponse);
        }
    }
}
