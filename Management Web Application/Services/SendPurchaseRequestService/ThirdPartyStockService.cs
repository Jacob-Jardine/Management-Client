using Management_Web_Application.DomainModel;
using Management_Web_Application.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

namespace Management_Web_Application.Services.PurchaseService
{
    /// <summary>
    /// Concrete implementation for interacting with third party stock service
    /// </summary>
    public class ThirdPartyStockService : IThirdPartyStockService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;

        /// <summary>
        /// Constructor setup for third party stock service
        /// </summary>
        /// <param name="config"></param>
        /// <param name="client"></param>
        public ThirdPartyStockService(IConfiguration config, HttpClient client)
        {
            _config = config;
            string baseUrl = config["PURCHASE_BASE_URL"];
            client.BaseAddress = new System.Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
        }

        /// <summary>
        /// Posts purchase request to third party stock service
        /// </summary>
        /// <param name="purchaseDomainModel"></param>
        /// <returns></returns>
        public async Task<bool> SendPurchaseRequest(SendPurchaseRequestDomainModel purchaseDomainModel)
        {
            if(purchaseDomainModel == null)
            {
                return false;
            }
            var json = JsonSerializer.Serialize(purchaseDomainModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("Send-Purchase-Request", data);
            if(response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}
