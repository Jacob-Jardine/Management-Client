using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.DomainModel
{
    public class AddAuth0PermissionsDomainModel
    {
        public string permission_name { get; set; }
        private string _resource_server_identifier = "https://thamco.com/";
        public string resource_server_identifier 
        {
            get 
            {
                return _resource_server_identifier;
            }
        }
    }
}
