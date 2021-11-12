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
    public class SendPurchaseRequestService : IPurchaseRequestService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;

        public SendPurchaseRequestService(IConfiguration config, HttpClient client)
        {
            _config = config;
            client.BaseAddress = _config.GetValue<Uri>("PURCHASE_BASE_URL");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
        }

        public async Task SendPurchaseRequest(PurchaseDomainModel purchaseDomainModel)
        {
            var json = JsonSerializer.Serialize(purchaseDomainModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("SendPurchaseRequest", data);
            response.EnsureSuccessStatusCode();
        }
    }
}
