using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.DomainModel
{
    public class SearchAuth0UserDoimainModel
    {
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string email { get; set; }

        public string user_id { get; set; }
    }
}
