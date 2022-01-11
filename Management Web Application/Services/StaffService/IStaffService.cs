using Management_Web_Application.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Services.StaffService
{
    /// <summary>
    /// Interface for staff service
    /// </summary>
    public interface IStaffService
    {
        public Task<IEnumerable<StaffDTO>> GetAllStaffAsync(string token);
        public Task<StaffDTO> GetStaffByIDAsnyc(int ID, string token);
        public Task<StaffDTO> CreateStaffAsync(StaffDTO staffDomainModel, string token);
        public Task<bool> UpdateStaff(StaffUpdateDTO staffDomainModel, string token);
        public Task<bool> DeleteStaff(int ID, string token);
    }
}
