using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Models
{
    public class StaffCreateViewModel
    {
        [Required]
        public int StaffID { get; set; }

        [Required(ErrorMessage = "Enter A First Name")]
        [StringLength(20, ErrorMessage = "First name can't be empty")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string StaffFirstName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "First name can't be empty")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string StaffLastName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$")]
        public string StaffEmailAddress { get; set; }
    }
}
