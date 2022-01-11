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
    /// <summary>
    /// Concrete implementation that interacts with the product service
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;

        /// <summary>
        /// Constructor instantiating configuring the HttpClient
        /// </summary>
        /// <param name="config"></param>
        /// <param name="client"></param>
        public ProductService(IConfiguration config, HttpClient client)
        {
            _config = config;
            string baseUrl = config["PRODUCT_BASE_URL"];
            client.BaseAddress = new System.Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
        }

        /// <summary>
        /// Sends a request to the product service to return product with ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="token"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Sends request to product service to get all prodcuts
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Sends request to the product service to post new product to product service
        /// </summary>
        /// <param name="postToProductServiceDomainModel"></param>
        /// <param name="token"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Sends request to product service to update product quantity
        /// </summary>
        /// <param name="updateProductQtyDomainModel"></param>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
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
