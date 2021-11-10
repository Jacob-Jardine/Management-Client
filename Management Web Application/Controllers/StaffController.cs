using AutoMapper;
using AutoMapper.Configuration;
using Management_Web_Application.DomainModel;
using Management_Web_Application.Models;
using Management_Web_Application.Services.StaffService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffService _staffService;
        private IMapper _mapper;
        
        public StaffController(IStaffService staffService, IMapper mapper)
        {
            _staffService = staffService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult<IEnumerable<StaffReadViewModel>>> GetAllStaff()
        {
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            try
            {
                var getAllStaff = await _staffService.GetAllStaffAsync();
                return View(_mapper.Map<IEnumerable<StaffReadViewModel>>(getAllStaff));
            }
            catch
            {
                return Redirect($"{baseURL}home/noaction");
            }
        }

        public async Task<ActionResult<StaffReadViewModel>> GetStaffByID(int ID)
        {
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            try
            {
                var getStaffByID = await _staffService.GetStaffByIDAsnyc(ID);
                if (getStaffByID != null)
                {
                    return View(_mapper.Map<StaffReadViewModel>(getStaffByID));
                }
                else
                {
                    return Redirect($"{baseURL}home/noaction");
                }
            }
            catch
            {
                return Redirect($"{baseURL}home/noaction");
            }
        }

        [HttpGet]
        public IActionResult CreateStaff() 
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateStaff(StaffCreateViewModel staffCreateViewModel)
        {
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            if (!ModelState.IsValid) 
            {
                return View(staffCreateViewModel);
            }
            try
            {
                var staffModel = _mapper.Map<StaffDomainModel>(staffCreateViewModel);
                StaffDomainModel newStaffDomainModel = await _staffService.CreateStaffAsync(staffModel);
                return Redirect($"{baseURL}staff/GetStaffById/{newStaffDomainModel.StaffID}");
            }
            catch
            {
                return Redirect($"{baseURL}home/noaction");
            }
        }

        [HttpGet]
        public async Task<ActionResult> UpdateStaff(int? ID, StaffUpdateViewModel staffUpdateViewModel)
        {
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            try
            {
                if (!ModelState.IsValid) 
                {
                    return View(staffUpdateViewModel);
                }
                var getStaffByID = await _staffService.GetStaffByIDAsnyc(ID);
                if (getStaffByID != null)
                {
                    return View(_mapper.Map<StaffUpdateViewModel>(getStaffByID));
                }
                else
                {
                    return Redirect($"{baseURL}home/noaction");
                }
            }
            catch
            {
                return Redirect($"{baseURL}home/noaction");
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateStaff(StaffUpdateViewModel staffUpdateViewModel)
        {
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            if (!ModelState.IsValid)
            {
                return View(staffUpdateViewModel);
            }
            try
            {
                var staffModel = _mapper.Map<StaffDomainModel>(staffUpdateViewModel);
                StaffDomainModel newStaffDomainModel = await _staffService.UpdateStaff(staffModel);
                newStaffDomainModel.StaffID = staffUpdateViewModel.StaffID;
                if (newStaffDomainModel != null) 
                {
                    return Redirect($"{baseURL}staff/GetStaffById/{newStaffDomainModel.StaffID}");
                }
                return Redirect($"{baseURL}home/noaction");
            }
            catch
            {
                return Redirect($"{baseURL}home/noaction");
            }
        }

        public async Task<ActionResult> DeleteStaff(int? ID)
        {
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            try
            {
                var getStaffByID = await _staffService.GetStaffByIDAsnyc(ID);
                if (getStaffByID != null)
                {
                    await _staffService.DeleteStaff(getStaffByID.StaffID);
                    return Redirect($"{baseURL}staff/GetAllStaff");
                }
                else
                {
                    return Redirect($"{baseURL}home/noaction");
                }
            }
            catch
            {
                return Redirect($"{baseURL}home/noaction");
            }
        }
    }
}
