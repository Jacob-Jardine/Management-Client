using Management_Web_Application.DomainModel;
using Management_Web_Application.Models.Auth0;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Management_Web_Application.Services.Auth0Service
{
    public class Auth0Service : IAuth0Service
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;

        public Auth0Service(IConfiguration config, HttpClient client)
        {
            _config = config;
            client.BaseAddress = _config.GetValue<Uri>("AUTH0_URL");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
        }

        public async Task CreateAuth0User(CreateAuth0UserDomainModel auth0DomainModel)
        {
            var token = authToken();

            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var json = JsonSerializer.Serialize(auth0DomainModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await _client.PostAsync("users", data);
            responses.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<SearchAuth0UserDoimainModel>> SearchByEmail(string email)
        {
            var token = authToken();

            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responses = await _client.GetAsync($"users-by-email?email={email}");
            if (responses.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            responses.EnsureSuccessStatusCode();
            var staff = await responses.Content.ReadAsAsync<IEnumerable<SearchAuth0UserDoimainModel>>();
            return staff;
        }

        public async Task<IEnumerable<AddAuth0PermissionsDomainModel>> UpdateAuth0UserPermissions(AddAuth0PermissionsDomainModels auth0DomainModel, string id)
        {

            var token = authToken();
            var json = JsonSerializer.Serialize(auth0DomainModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var responses = await _client.PostAsync($"users/{id}/permissions", data);
            if (responses.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            responses.EnsureSuccessStatusCode();
            var staff = await responses.Content.ReadAsAsync<IEnumerable<AddAuth0PermissionsDomainModel>>();
            return staff;
        }

        public async Task<IEnumerable<ReadAuth0PermissionsDomainModel>> ReadAuth0Permissions(string id)
        {
            //_client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await _client.GetAsync($"users/{id}/permissions");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var staff = await response.Content.ReadAsAsync<IEnumerable<ReadAuth0PermissionsDomainModel>>();
            return staff;
        }

        private string authToken()
        {
            var client = new RestClient("https://thamco-group-a.eu.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"client_id\":\"VZZcF5zG7yyJ7J1lrO53MEUiHYJnbsCc\",\"client_secret\":\"cFFDfQ5q6iRS4UGtxAj_rfkxOvTpZ7fa42yZiIzXMfymXp9zEgIVm8h9c6dZF8_a\",\"audience\":\"https://thamco-group-a.eu.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return JsonSerializer.Deserialize<Auth0DesirializeResponseModel>(response.Content).access_token;
        }
    }
}

