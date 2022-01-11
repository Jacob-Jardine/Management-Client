using AutoMapper;
using Management_Web_Application.DomainModel;
using Management_Web_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Profiles
{
    public class StaffProfile : Profile
    {
        public StaffProfile()
        {
            CreateMap<StaffDTO, StaffReadViewModel>();
            CreateMap<StaffCreateViewModel, StaffDTO>();
            CreateMap<StaffDTO, StaffUpdateViewModel>();
            CreateMap<StaffUpdateViewModel, StaffDTO>();
            CreateMap<StaffDTO, StaffUpdateViewModel>();
            CreateMap<StaffUpdateDTO, StaffDTO>();
            CreateMap<StaffDTO, StaffUpdateDTO>();
            CreateMap<StaffUpdateDTO, StaffUpdateViewModel>();
            CreateMap<StaffUpdateViewModel, StaffUpdateDTO>();
        }
    }
}
