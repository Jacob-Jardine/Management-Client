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

        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter A First Name")]
        [StringLength(20, ErrorMessage = "First name can't be empty")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string StaffFirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter A Last Name")]
        [StringLength(20, ErrorMessage = "First name can't be empty")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string StaffLastName { get; set; }

        [Display(Name = "Email Address Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter An Email Address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Please Use Conventional Email Formatting example@example")]
        public string StaffEmailAddress { get; set; }
    }
}
