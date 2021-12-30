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
using Management_Web_Application.Services.ProductService;

namespace Management_Web_Application.Services.PurchaseService
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;

        public ProductService(IConfiguration config, HttpClient client)
        {
            _config = config;
            client.BaseAddress = _config.GetValue<Uri>("PRODUCT_BASE_URLL");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
        }

        public async Task UpdateProductQty(UpdateProductQtyDomainModel updateProductQtyDomainModel,int id, string token)
        {
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await _client.GetAsync($"/api/product/{id}/stock");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }
            response.EnsureSuccessStatusCode();
            return;
        }
    }
}
