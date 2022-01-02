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
        private readonly List<StaffDomainModel> _staffList;

        public FakeStaffService() 
        {
            _staffList = new List<StaffDomainModel>()
            {
                new StaffDomainModel() {StaffID = 1, StaffFirstName = "Jacob", StaffLastName = "Jardine", StaffEmailAddress = "Jacob.Jardine@ThAmCo.co.uk"},
                new StaffDomainModel() {StaffID = 2, StaffFirstName = "Ben", StaffLastName = "Souch", StaffEmailAddress = "Ben.Souch@ThAmCo.co.uk"},
                new StaffDomainModel() {StaffID = 3, StaffFirstName = "Joseph", StaffLastName = "Stavers", StaffEmailAddress = "Joseph.Stavers@ThAmCo.co.uk"},
                new StaffDomainModel() {StaffID = 4, StaffFirstName = "Teddy", StaffLastName = "Teasdale", StaffEmailAddress = "Teddy.Teasdale@ThAmCo.co.uk"},
                new StaffDomainModel() {StaffID = 5, StaffFirstName = "Cristian", StaffLastName = "Tudor", StaffEmailAddress = "Cristian.Tudor@ThAmCo.co.uk"},
                new StaffDomainModel() {StaffID = 6, StaffFirstName = "Auth0", StaffLastName = "Auth0", StaffEmailAddress = "jacob@thamco.com"}
            };
        }

        public Task<IEnumerable<StaffDomainModel>> GetAllStaffAsync(string token) => Task.FromResult(_staffList.AsEnumerable());

        public Task<StaffDomainModel> GetStaffByIDAsnyc(int ID, string token) => Task.FromResult(_staffList.FirstOrDefault(x => x.StaffID == ID));

        public Task<StaffDomainModel> CreateStaffAsync(StaffDomainModel staffDomainModel, string token)
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

        public Task<StaffDomainModel> UpdateStaff(StaffDomainModel staffDomainModel)
        {
            var oldStaffDomainModel = GetStaffByIDAsnyc(staffDomainModel.StaffID, "");
            _staffList.RemoveAll(x => x.StaffID == oldStaffDomainModel.Result.StaffID);
            _staffList.Add(staffDomainModel);
            return Task.FromResult(staffDomainModel);
        }

        public Task DeleteStaff(int ID)
        {
            var deleteStaffDomainModel = GetStaffByIDAsnyc(ID, "");
            _staffList.RemoveAll(x => x.StaffID == deleteStaffDomainModel.Result.StaffID);
            return Task.CompletedTask;
        }
        private int GetStaffID()
        {
            return (_staffList == null && _staffList.Count() == 0) ? 1 : _staffList.Max(x => x.StaffID) + 1;
        }
    }
}
