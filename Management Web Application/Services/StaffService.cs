﻿using Management_Web_Application.DomainModel;
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

namespace Management_Web_Application.Services
{
    public class StaffService : IStaffService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client;

        public StaffService(IConfiguration config, HttpClient client) 
        {
            _config = config;
            client.BaseAddress = _config.GetValue<Uri>("STAFF_BASE_URL");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
        }

        public async Task<IEnumerable<StaffDomainModel>> GetAllStaffAsync()
        {
            var response = await _client.GetAsync("GetAllStaff");
            if (response.StatusCode == HttpStatusCode.NotFound) 
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var staff = await response.Content.ReadAsAsync<IEnumerable<StaffDomainModel>>();
            return staff;
        }

        public async Task<StaffDomainModel> GetStaffByIDAsnyc(int? ID)
        {
            var response = await _client.GetAsync($"{ID}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var staff = await response.Content.ReadAsAsync<StaffDomainModel>();
            return staff;
        }

        public async Task<StaffDomainModel> CreateStaffAsync(StaffDomainModel staffDomainModel)
        {
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

        public async Task<StaffDomainModel> UpdateStaff(StaffDomainModel staffDomainModel)
        {
            var json = JsonSerializer.Serialize(staffDomainModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"update/{staffDomainModel.StaffID}", data);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            var emptyDomainModel = new StaffDomainModel();
            return emptyDomainModel;
        }

        public async Task DeleteStaff(int? ID)
        {
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
