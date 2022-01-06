using Management_Web_Application.DomainModel;
using Management_Web_Application.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.Net.Http.Headers;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace Management_Web_Application.Services.StaffService
{
    /// <summary>
    /// Concrete implentation for staff service requests
    /// </summary>
    public class StaffService : IStaffService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;

        /// <summary>
        /// Concrete implementation that interacts with the staff service
        /// </summary>
        /// <param name="config"></param>
        /// <param name="client"></param>
        public StaffService(IConfiguration config, HttpClient client) 
        {
            _config = config;
            client.BaseAddress = _config.GetValue<Uri>("STAFF_BASE_URL");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
        }

        /// <summary>
        /// Sends a get request to the staff service to get all staff accounts
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IEnumerable<StaffDomainModel>> GetAllStaffAsync(string token)
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            var response = await _client.GetAsync("GetAllStaff");
            if (response.StatusCode == HttpStatusCode.NotFound) 
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var staff = await response.Content.ReadAsAsync<IEnumerable<StaffDomainModel>>();
            return staff;
        }

        /// <summary>
        /// Sends a get request to the staff service to get a staff member by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<StaffDomainModel> GetStaffByIDAsnyc(int ID, string token)
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            var response = await _client.GetAsync($"{ID}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var staff = await response.Content.ReadAsAsync<StaffDomainModel>();
            return staff;
        }

        /// <summary>
        /// Sends a post request to the staff service to add a new staff member
        /// </summary>
        /// <param name="staffDomainModel"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<StaffDomainModel> CreateStaffAsync(StaffDomainModel staffDomainModel, string token)
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
                
            var json = JsonSerializer.Serialize(staffDomainModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("CreateStaff", data);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var staff = await response.Content.ReadAsAsync<StaffDomainModel>();
            return staff;
        }

        /// <summary>
        /// Sends a put request to the staff service to update a staff account
        /// </summary>
        /// <param name="staffDomainModel"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> UpdateStaff(StaffUpdateDomainModel staffDomainModel, string token)
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            var json = JsonSerializer.Serialize(staffDomainModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"update/{staffDomainModel.StaffID}", data);
            if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Sends a delete request to the staff service to delete a staff account
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task DeleteStaff(int ID, string token)
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            var response = await _client.DeleteAsync($"delete/{ID}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }
            response.EnsureSuccessStatusCode();
            return;
        }
    }
}

