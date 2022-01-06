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
using System.Web;

namespace Management_Web_Application.Services.Auth0Service
{
    /// <summary>
    /// Concrete implementation that interacts with Auth0
    /// </summary>
    public class Auth0Service : IAuth0Service
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;

        /// <summary>
        /// Constructor where config and client are instantiated
        /// </summary>
        /// <param name="config"></param>
        /// <param name="client"></param>
        public Auth0Service(IConfiguration config, HttpClient client)
        {
            _config = config;
            client.BaseAddress = _config.GetValue<Uri>("AUTH0_URL");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
        }

        /// <summary>
        /// Sends a post request to Auth0 to create a user
        /// </summary>
        /// <param name="auth0DomainModel"></param>
        /// <returns></returns>
        public async Task CreateAuth0User(CreateAuth0UserDomainModel auth0DomainModel)
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                var token = authToken();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            var json = JsonSerializer.Serialize(auth0DomainModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await _client.PostAsync("users", data);
            responses.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Sends a get request to Auth0 to find a user with the email parameter
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SearchAuth0UserDoimainModel>> SearchByEmail(string email)
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                var token = authToken();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            var responses = await _client.GetAsync($"users-by-email?email={email}");
            if (responses.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            responses.EnsureSuccessStatusCode();
            var staff = await responses.Content.ReadAsAsync<IEnumerable<SearchAuth0UserDoimainModel>>();
            return staff;
        }

        /// <summary>
        /// Sends a post request to Auth0 to add new permissions to a user
        /// </summary>
        /// <param name="auth0DomainModel"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAuth0UserPermissions(AddAuth0PermissionsDomainModels auth0DomainModel, string id)
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                var token = authToken();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            var json = JsonSerializer.Serialize(auth0DomainModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var responses = await _client.PostAsync($"users/{id}/permissions", data);
            if (responses.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            responses.EnsureSuccessStatusCode();
            return true;
        }

        /// <summary>
        /// Sends a post request to Auth0 to remove permissions from a user
        /// </summary>
        /// <param name="auth0DomainModel"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAuth0Permissions(AddAuth0PermissionsDomainModels auth0DomainModel, string id)
        {
            var token = authToken();
            var json = JsonSerializer.Serialize(auth0DomainModel);
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddJsonBody(json);
            IRestClient restClient = new RestClient($"https://dev-03ydhf5b.us.auth0.com/api/v2/users/{id}/permissions");
            IRestResponse restResponse = restClient.Delete(request);

            if(restResponse.StatusCode != HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sends a get request to Auth0 to get a list of all the permissions a user has
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ReadAuth0PermissionsDomainModel>> ReadAuth0Permissions(string id)
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                var token = authToken();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            var response = await _client.GetAsync($"users/{id}/permissions");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var staff = await response.Content.ReadAsAsync<IEnumerable<ReadAuth0PermissionsDomainModel>>();
            return staff;
        }

        /// <summary>
        /// Sends a delete request to Auth0 t0 remove a user
        /// </summary>
        /// <param name="Auth0Id"></param>
        /// <returns></returns>
        public async Task DeleteAuth0User(string Auth0Id)
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                var token = authToken();
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            var responses = await _client.DeleteAsync($"users/{Auth0Id}");
            responses.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Sends a request to Auth0 for a new token
        /// </summary>
        /// <returns></returns>
        private string authToken()
        {
            var client = new RestClient("https://dev-03ydhf5b.us.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"client_id\":\"kRm1S9F3MPcH0klqFrWa0spHVhurjFiZ\",\"client_secret\":\"7mn6rdFNSlRktBsV-dLFfXSRyU8qT8ipQbf0dse5-KwaVaL0bkIX2dhUZWgChTRJ\",\"audience\":\"https://dev-03ydhf5b.us.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return JsonSerializer.Deserialize<Auth0DesirializeResponseModel>(response.Content).access_token;
        }
    }
}

