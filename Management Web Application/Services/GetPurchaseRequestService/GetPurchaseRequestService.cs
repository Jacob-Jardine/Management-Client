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
using Management_Web_Application.Services.PurchaseService;

namespace Management_Web_Application.Services.GetPurchaseRequestService
{
    public class GetPurchaseRequestService : IGetPurchaseRequestService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;

        public GetPurchaseRequestService(IConfiguration config, HttpClient client)
        {
            _config = config;
            client.BaseAddress = _config.GetValue<Uri>("PURCHASE_BASE_URL");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
        }

        public async Task<IEnumerable<GetPurchaseRequestDomainModel>> GetAllPurchaseAsync()
        {
            var response = await _client.GetAsync("GetPurchaseRequests");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var purchaseRequests = await response.Content.ReadAsAsync<IEnumerable<GetPurchaseRequestDomainModel>>();
            return purchaseRequests;
        }   
    }
}
