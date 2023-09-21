using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sender.Core.Models.ApiModels;
using Sender.Core.Models.TsoftModels;
using Sender.Infrastructure.Services.Tsoft;
using System.Runtime.InteropServices;

namespace Sender.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> logger;
        private readonly TsoftClient tsoftClient;

        public ProductController(ILogger<ProductController> logger, TsoftClient tsoftClient)
        {
            this.logger = logger;
            this.tsoftClient = tsoftClient;
        }
        [HttpPost]
        public async Task<IActionResult> GetTsoftProducts([FromForm] SiteUser siteUser)
        {
            //var siteUser = new SiteUser()
            //{
            //    BaseUrl = "https://arge-talhabuyukkose.1isim.com",
            //    UserName = "talha",
            //    Password = "Talha.01"
            //};
            var tsoftProductModel = await tsoftClient.GetTsoftProductAll(siteUser);

            return Ok(tsoftProductModel.data);
    }
}
}
