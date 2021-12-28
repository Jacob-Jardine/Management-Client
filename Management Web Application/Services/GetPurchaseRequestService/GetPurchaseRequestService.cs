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
using Management_Web_Application.Models.PurchaseModels;

namespace Management_Web_Application.Services.GetPurchaseRequestService
{
    public class GetPurchaseRequestService : IGetPurchaseRequestService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;

        public GetPurchaseRequestService(IConfiguration config, HttpClient client)
        {
            _config = config;
            client.BaseAddress = _config.GetValue<Uri>("GET_PURCHASE_URL");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;            
        }

        public async Task<IEnumerable<GetPurchaseRequestDomainModel>> GetAllPurchaseAsync(string token)
        {
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await _client.GetAsync("");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var purchaseRequests = await response.Content.ReadAsAsync<IEnumerable<GetPurchaseRequestDomainModel>>();
            return purchaseRequests;
        }

        public async Task<GetPurchaseRequestDomainModel> GetPurchaseRequestByIdAsync(int? ID, string token)
        {
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await _client.GetAsync($"{ID}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var purchaseRequest = await response.Content.ReadAsAsync<GetPurchaseRequestDomainModel>();
            return purchaseRequest;
        }

        public async Task UpdatePurchaseRequestStatus(GetPurchaseRequestDomainModel pruchaseRequestDomainModel, string token, int status)
        {
            var patchModel = new PurchasePatchModel()
            {
                op = "replace",
                path = "/PurchaseRequestStatus",
                value = status
            };
            List<PurchasePatchModel> purchasePatchModels = new List<PurchasePatchModel>
            {
                patchModel
            };
            //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var json = JsonSerializer.Serialize(purchasePatchModels);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync($"{pruchaseRequestDomainModel.purchaseRequestID}", data);
            response.EnsureSuccessStatusCode();
        }
    }
}
