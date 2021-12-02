using AutoMapper;
using AutoMapper.Configuration;
using Management_Web_Application.DomainModel;
using Management_Web_Application.Models;
using Management_Web_Application.Services.StaffService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Management_Web_Application.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffService _staffService;
        private IMapper _mapper;
        private readonly string _baseURL = Environment.GetEnvironmentVariable("BASE_URL");

        public StaffController(IStaffService staffService, IMapper mapper)
        {
            _staffService = staffService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult<IEnumerable<StaffReadViewModel>>> GetAllStaff()
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var getAllStaff = await _staffService.GetAllStaffAsync(accessToken);
                return View(_mapper.Map<IEnumerable<StaffReadViewModel>>(getAllStaff));
            }
            catch(Exception e)
            {
                int x = 1;
                return Redirect($"{_baseURL}home/noaction");
            }
        }

        public async Task<ActionResult<StaffReadViewModel>> GetStaffByID(int ID)
        {
            try
            {
                var getStaffByID = await _staffService.GetStaffByIDAsnyc(ID);
                if (getStaffByID != null)
                {
                    return View(_mapper.Map<StaffReadViewModel>(getStaffByID));
                }
                else
                {
                    return Redirect($"{_baseURL}home/noaction");
                }
            }
            catch
            {
                return Redirect($"{_baseURL}home/noaction");
            }
        }

        [HttpGet]
        public IActionResult CreateStaff() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateStaff(StaffCreateViewModel staffCreateViewModel)
        {
            if (!ModelState.IsValid) 
            {
                return View(staffCreateViewModel);
            }
            try
            {
                var staffModel = _mapper.Map<StaffDomainModel>(staffCreateViewModel);
                StaffDomainModel newStaffDomainModel = await _staffService.CreateStaffAsync(staffModel);
                return Redirect($"{_baseURL}staff/GetStaffById/{newStaffDomainModel.StaffID}");
            }
            catch
            {
                return Redirect($"{_baseURL}home/noaction");
            }
        }

        [HttpGet]
        public async Task<ActionResult> UpdateStaff(int? ID, StaffUpdateViewModel staffUpdateViewModel)
        {
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
                    return Redirect($"{_baseURL}home/noaction");
                }
            }
            catch
            {
                return Redirect($"{_baseURL}home/noaction");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateStaff(StaffUpdateViewModel staffUpdateViewModel)
        {
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
                    return Redirect($"{_baseURL}staff/GetStaffById/{newStaffDomainModel.StaffID}");
                }
                return Redirect($"{_baseURL}home/noaction");
            }
            catch
            {
                return Redirect($"{_baseURL}home/noaction");
            }
        }

        public async Task<ActionResult> DeleteStaff(int? ID)
        {
            try
            {
                var getStaffByID = await _staffService.GetStaffByIDAsnyc(ID);
                if (getStaffByID != null)
                {
                    await _staffService.DeleteStaff(getStaffByID.StaffID);
                    return Redirect($"{_baseURL}staff/GetAllStaff");
                }
                else
                {
                    return Redirect($"{_baseURL}home/noaction");
                }
            }
            catch
            {
                return Redirect($"{_baseURL}home/noaction");
            }
        }
    }
}
