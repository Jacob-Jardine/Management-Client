using Management_Web_Application.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Services.Auth0Service
{
    public interface IAuth0Service
    {
        public Task CreateAuth0User(CreateAuth0UserDomainModel auth0DomainModel);

        public Task<IEnumerable<SearchAuth0UserDoimainModel>> SearchByEmail(string email);

        public Task<IEnumerable<AddAuth0PermissionsDomainModel>> UpdateAuth0UserPermissions(AddAuth0PermissionsDomainModels auth0DomainModel, string id);

        public Task<IEnumerable<ReadAuth0PermissionsDomainModel>> ReadAuth0Permissions(string id);
    }
}
