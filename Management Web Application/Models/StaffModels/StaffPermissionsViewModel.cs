using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Models.StaffModels
{
    public class StaffPermissionsViewModel
    {
        [Display(Name = "Customer Account Deleltion Request")]
        public bool CustomerAccountDeleltionRequestBool { get; set; }
    }
}
