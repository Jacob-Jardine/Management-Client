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
            client.BaseAddress = _config.GetValue<Uri>("PRODUCT_BASE_URL");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
        }

        public async Task<PostToProductServiceDomainModel> GetProductById(int ID, string token)
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            var response = await _client.GetAsync($"/api/products/{ID}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            var staff = await response.Content.ReadAsAsync<PostToProductServiceDomainModel>();
            return staff;
        }

        public async Task<IEnumerable<PostToProductServiceDomainModel>> GetProducts(string token)
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            var response = await _client.GetAsync($"/api/products/");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            var staff = await response.Content.ReadAsAsync<IEnumerable<PostToProductServiceDomainModel>>();
            return staff;
        }

        public async Task<bool> PostProduct(PostToProductServiceDomainModel postToProductServiceDomainModel, string token)
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            var json = JsonSerializer.Serialize(postToProductServiceDomainModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/products/", data);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task UpdateProductQty(UpdateProductQtyDomainModel updateProductQtyDomainModel,int id, string token)
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            var json = JsonSerializer.Serialize(updateProductQtyDomainModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"/api/products/{id}/stock", data);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }
            response.EnsureSuccessStatusCode();
            return;
        }
    }
}
