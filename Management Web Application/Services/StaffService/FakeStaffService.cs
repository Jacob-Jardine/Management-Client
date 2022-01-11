using Management_Web_Application.DomainModel;
using Management_Web_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Services.StaffService
{
    public class FakeStaffService : IStaffService
    {
        public List<StaffDTO> _staffList;

        public FakeStaffService() 
        {
            _staffList = new List<StaffDTO>()
            {
                new StaffDTO() {StaffID = 1, StaffFirstName = "Jacob", StaffLastName = "Jardine", StaffEmailAddress = "Jacob.Jardine@ThAmCo.co.uk"}
            };
        }

        public Task<IEnumerable<StaffDTO>> GetAllStaffAsync(string token) => Task.FromResult(_staffList.AsEnumerable());

        public Task<StaffDTO> GetStaffByIDAsnyc(int ID, string token) => Task.FromResult(_staffList.FirstOrDefault(x => x.StaffID == ID));

        public Task<StaffDTO> CreateStaffAsync(StaffDTO staffDomainModel, string token)
        {
            var emailCheck = _staffList.Any(x => x.StaffEmailAddress == staffDomainModel.StaffEmailAddress);
            if(emailCheck == false) 
            {
                int newStaffID = GetStaffID();
                staffDomainModel.StaffID = newStaffID;
                staffDomainModel.StaffEmailAddress.ToLower();
                _staffList.Add(staffDomainModel);
                return Task.FromResult(staffDomainModel);
            }
            return null;
        }

        public async Task<bool> UpdateStaff(StaffUpdateDTO staffDomainModel, string token)
        {
            var oldStaffDomainModel = GetStaffByIDAsnyc(staffDomainModel.StaffID, "");
            _staffList.RemoveAll(x => x.StaffID == oldStaffDomainModel.Result.StaffID);
            var toList = new StaffDTO();
            toList.StaffID = staffDomainModel.StaffID;
            toList.StaffFirstName = staffDomainModel.StaffFirstName;
            toList.StaffLastName = staffDomainModel.StaffLastName;
            _staffList.Add(toList);
            return true;
        }

        public async Task<bool> DeleteStaff(int ID, string token)
        {
            try
            {
                var deleteStaffDomainModel = GetStaffByIDAsnyc(ID, "");
                _staffList.RemoveAll(x => x.StaffID == deleteStaffDomainModel.Result.StaffID);
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        private int GetStaffID()
        {
            return (_staffList == null && _staffList.Count() == 0) ? 1 : _staffList.Max(x => x.StaffID) + 1;
        }
    }
}
