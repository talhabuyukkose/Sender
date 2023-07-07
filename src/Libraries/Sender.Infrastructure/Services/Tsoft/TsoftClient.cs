using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Sender.Core.Constants;
using Sender.Core.Constants.T_Soft;
using Sender.Core.Extensions.JsonProcess;
using Sender.Core.Interfaces;
using Sender.Core.Models;
using Sender.Core.Models.MemoryModels;
using Sender.Core.Models.T_SoftModels;
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

        public TsoftClient(HttpClient httpClient, IMemoryService memoryService)
        {
            this.client = httpClient;
            this.memoryService = memoryService;
        }

        private async Task<string> GetToken(SiteUser user)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(user.BaseUrl, nameof(user.BaseUrl));
            ArgumentNullException.ThrowIfNullOrEmpty(user.UserName, nameof(user.UserName));
            ArgumentNullException.ThrowIfNullOrEmpty(user.Password, nameof(user.Password));

            var urlWithEndPoint = new EndpointBuilder(user.BaseUrl)
                .Append("rest1")
                .Append(ConstantTsoftEndPoints.AuthLogin)
                .Append(user.UserName).Build();


            if (memoryService.TryGetValue(user.BaseUrl, out var memoryData))
            {
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

            return (memoryData as MemoryWebSiteModel).Token;
        }

        public async Task<TsoftBaseModel> SendImage(SiteUser user, byte[] byteFile, string fileName, string productCode)
        {
            ArgumentNullException.ThrowIfNull(byteFile, nameof(byteFile));
            ArgumentNullException.ThrowIfNullOrEmpty(fileName, nameof(fileName));
            ArgumentNullException.ThrowIfNullOrEmpty(productCode, nameof(productCode));

            string token = string.Empty;

            if (memoryService.TryGetValue(user.BaseUrl, out var memoryData))
            {
                token = ((MemoryWebSiteModel)memoryData).Token;
            }
            else
            {
                token = await this.GetToken(user);
            }
            var urlWithEndPoint = new EndpointBuilder(user.BaseUrl)
                .Append("rest1")
                .Append(ConstantTsoftEndPoints.AddImageFromFile)
                .AppendParam("ProductCode", productCode).Build();

            var fileContent = new ByteArrayContent(byteFile);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

            var multipartFromDataContent = new MultipartFormDataContent();
            multipartFromDataContent.Add(fileContent, "image", fileName);
            multipartFromDataContent.Add(new StringContent(token), "token");

            var responseMessage = await client.PostAsync(urlWithEndPoint, multipartFromDataContent);

            return await responseMessage.Content.ReadFromJsonAsync<TsoftBaseModel>();
        }
    }
}
