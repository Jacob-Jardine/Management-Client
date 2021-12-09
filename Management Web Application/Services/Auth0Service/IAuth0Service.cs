using Management_Web_Application.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Services.Auth0Service
{
    public interface IAuth0Service
    {
        public Task<CreateAuth0UserDomainModel> CreateAuth0User(CreateAuth0UserDomainModel auth0DomainModel);
    }
}
