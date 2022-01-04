using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Models
{
    public class StaffUpdateViewModel
    {
        public int StaffID { get; set; }

        [StringLength(20, ErrorMessage = "First name can't be empty")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string StaffFirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [StringLength(20, ErrorMessage = "Last name can't be empty")]
        public string StaffLastName { get; set; }
    }
}
