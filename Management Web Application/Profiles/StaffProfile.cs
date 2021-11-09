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
            CreateMap<StaffDomainModel, StaffReadViewModel>();
            CreateMap<StaffCreateViewModel, StaffDomainModel>();
            CreateMap<StaffDomainModel, StaffUpdateViewModel>();
            CreateMap<StaffUpdateViewModel, StaffDomainModel>();
            CreateMap<StaffDomainModel, StaffUpdateViewModel>();
        }
    }
}
