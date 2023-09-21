using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Sender.Core.Constants;
using Sender.Core.Constants.Tsoft;
using Sender.Core.Extensions.JsonProcess;
using Sender.Core.Interfaces;
using Sender.Core.Models.ApiModels;
using Sender.Core.Models.MemoryModels;
using Sender.Core.Models.TsoftModels;
using Sender.Infrastructure.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Sender.Infrastructure.Services.Tsoft
{
    public class TsoftClient
    {
        private readonly HttpClient client;
        private readonly IMemoryService memoryService;
        private readonly ILogger<TsoftClient> logger;

        public TsoftClient(HttpClient httpClient, IMemoryService memoryService, ILogger<TsoftClient> logger)
        {
            this.client = httpClient;
            this.memoryService = memoryService;
            this.logger = logger;
        }

        private async Task<string> GetToken(SiteUser user)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(user.BaseUrl, nameof(user.BaseUrl));
            ArgumentNullException.ThrowIfNullOrEmpty(user.UserName, nameof(user.UserName));
            ArgumentNullException.ThrowIfNullOrEmpty(user.Password, nameof(user.Password));

            var urlWithEndPoint = new EndpointBuilder(user.BaseUrl)
                .Append("rest1")
                .Append(ConstantTsoftEndPoints.AuthLogin)
                .Append(user.UserName)
                .Build();


            if (memoryService.TryGetValue(user.BaseUrl, out var memoryData))
            {
                logger.LogInformation("Token received from MemoryCacheService");
                return ((MemoryWebSiteModel)memoryData).Token;
            }

            var content = new Dictionary<string, string>()
                {
                    {"pass",user.Password }
                };

            var httpres = await client.PostAsync(urlWithEndPoint, new FormUrlEncodedContent(content));

            // Ensure the response was successful, or throw an exception
            httpres.EnsureSuccessStatusCode();

            var responseModel = await httpres.Content.ReadFromJsonAsync<TsoftLoginModel>();

            var responseModelData = responseModel.data.FirstOrDefault();

            memoryData = new MemoryWebSiteModel()
            {
                WebSiteUrl = user.BaseUrl,
                Token = responseModelData.token,
            };

            DateTime.TryParse(responseModelData.expirationTime, out DateTime dateTimeResult);

            memoryService.CreateEntry(user.BaseUrl);
            memoryService.Set(user.BaseUrl, memoryData, dateTimeResult.AddHours(-1));


            logger.LogInformation($"Token received from {user.BaseUrl}");

            return (memoryData as MemoryWebSiteModel).Token;
        }


        public async Task<SendImageFromUrl> SendImageFromUrl(SiteUser user, SendImageFromUrl content)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(user));
            ArgumentNullException.ThrowIfNull(nameof(content));

            string token = await this.GetToken(user);

            var url = new EndpointBuilder(user.BaseUrl)
                .Append("rest1")
                .Append(ConstantTsoftEndPoints.AddImageFromLink)
                .Build();

            var keyValuePair = new Dictionary<string, string>
            {
                {"token",token},
                { "data",content.JsonSerialize() }
            };

            var responseMessage = await client.PostAsync(url, new FormUrlEncodedContent(keyValuePair));

            var tsoftBaseModel = await responseMessage.Content.ReadFromJsonAsync<TsoftBaseModel2>();

            if (tsoftBaseModel.message.FirstOrDefault().type == 1)
            {
                logger.LogInformation($"{content.ProductCode} kodlu ürün resmi başarıyla eklendi.");
                return default(SendImageFromUrl);
            }
            else
            {
                logger.LogError($"{content.ProductCode} kodlu ürün resmi yüklenemedi. Message : {string.Join(Environment.NewLine, tsoftBaseModel.message.FirstOrDefault().text)}");
                return content;
            }
        }

        public async Task<TsoftBaseModel> SendImage(SiteUser user, byte[] byteFile, string productCode)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(user.BaseUrl, nameof(user.BaseUrl));
            ArgumentNullException.ThrowIfNull(byteFile, nameof(byteFile));
            ArgumentNullException.ThrowIfNullOrEmpty(productCode, nameof(productCode));

            string token = await this.GetToken(user);

            var urlWithEndPoint = new EndpointBuilder(user.BaseUrl)
                .Append("rest1")
                .Append(ConstantTsoftEndPoints.AddImageFromFile)
                .AppendParam("ProductCode", productCode)
                .Build();

            var fileContent = new ByteArrayContent(byteFile);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

            var multipartFromDataContent = new MultipartFormDataContent();
            multipartFromDataContent.Add(fileContent, "image", productCode);
            multipartFromDataContent.Add(new StringContent(token), "token");

            var responseMessage = await client.PostAsync(urlWithEndPoint, multipartFromDataContent);

            var tsoftBaseModel = await responseMessage.Content.ReadFromJsonAsync<TsoftBaseModel>();

            if (tsoftBaseModel.message.FirstOrDefault().type == 1)
            {
                logger.LogInformation($"{productCode} kodlu ürün resmi başarıyla eklendi.");
            }
            else
            {
                logger.LogError($"{productCode} kodlu ürün resmi yüklenemedi. Message : {string.Join(Environment.NewLine, tsoftBaseModel.message.FirstOrDefault().text)}");
            }

            return await responseMessage.Content.ReadFromJsonAsync<TsoftBaseModel>();
        }

        public async Task<TsoftProductModel> GetProducts(SiteUser user, int start, int limit)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(user.BaseUrl, nameof(user.BaseUrl));

            string token = await this.GetToken(user);

            var urlEndpointBuilder = new EndpointBuilder(user.BaseUrl)
                .Append("rest1")
                .Append(ConstantTsoftEndPoints.GetProducts)
                .Build();

            var keyValuePairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("token",token),
                    new KeyValuePair<string, string>("limit",limit.ToString()),
                    new KeyValuePair<string, string>("start",start.ToString())
                };

            var httpresponse = await client.PostAsync(urlEndpointBuilder, new FormUrlEncodedContent(keyValuePairs));

            return await httpresponse.Content.ReadFromJsonAsync<TsoftProductModel>(); ;
        }
        public async Task<TsoftProductModel> GetTsoftProductAll(SiteUser siteUser)
        {
            TsoftProductModel tsoftProductModel = new TsoftProductModel();
            //var siteUser = new SiteUser()
            //{
            //    BaseUrl = "https://arge-talhabuyukkose.1isim.com",
            //    UserName = "talha",
            //    Password = "Talha.01"
            //};
            var totalproductcount = await GetProductTotalCount(siteUser);

            int limit = 100;
            for (int i = 0; i < totalproductcount / limit + 1; i++)
            {
                var responsemodel = await GetProducts(siteUser, limit * i, limit);

                tsoftProductModel.data.AddRange(responsemodel.data);
            }

            return tsoftProductModel;
        }
        public async Task<int> GetProductTotalCount(SiteUser user)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(user.BaseUrl, nameof(user.BaseUrl));

            string token = await GetToken(user);

            var urlEndpointBuilder = new EndpointBuilder(user.BaseUrl)
                .Append("rest1")
                .Append(ConstantTsoftEndPoints.GetProductTotal)
                .Build();

            var keyValuePairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("token",token)
            };

            var httpresponse = await client.PostAsync(urlEndpointBuilder, new FormUrlEncodedContent(keyValuePairs));

            var responsemodel = await httpresponse.Content.ReadFromJsonAsync<TsoftBaseModel>();

            return (int)responsemodel.summary.totalRecordCount;
        }
    }
}
