﻿using Management_Web_Application.DomainModel;
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

        public async Task<CreateAuth0UserDomainModel> CreateAuth0User(CreateAuth0UserDomainModel auth0DomainModel)
        {
            var token = authToken();

            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var json = JsonSerializer.Serialize(auth0DomainModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await _client.PostAsync("users", data);
            if (responses.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            responses.EnsureSuccessStatusCode();
            var staff = await responses.Content.ReadAsAsync<CreateAuth0UserDomainModel>();
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
