using AutoMapper;
using Management_Web_Application.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Profiles
{
    public class Auth0Profile : Profile
    {
        public Auth0Profile()
        {
            CreateMap<CreateAuth0UserDomainModel, CreateUserDomainModel>();
            CreateMap<CreateUserDomainModel, CreateAuth0UserDomainModel>();
        }
    }
}
