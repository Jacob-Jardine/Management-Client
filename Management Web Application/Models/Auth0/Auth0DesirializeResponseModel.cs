﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Models.Auth0
{
    public class Auth0DesirializeResponseModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
    }
}
