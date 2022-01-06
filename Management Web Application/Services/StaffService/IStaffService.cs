using Management_Web_Application.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Services.StaffService
{
    public interface IStaffService
    {
        public Task<IEnumerable<StaffDomainModel>> GetAllStaffAsync(string token);
        public Task<StaffDomainModel> GetStaffByIDAsnyc(int ID, string token);
        public Task<StaffDomainModel> CreateStaffAsync(StaffDomainModel staffDomainModel, string token);
        public Task<bool> UpdateStaff(StaffUpdateDomainModel staffDomainModel, string token);
        public Task DeleteStaff(int ID, string token);
    }
}
