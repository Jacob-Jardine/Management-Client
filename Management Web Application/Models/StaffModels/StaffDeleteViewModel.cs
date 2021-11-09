using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Models
{
    public class StaffDeleteViewModel
    {
        public int StaffID { get; set; }

        public string StaffFirstName { get; set; }

        public string StaffLastName { get; set; }

        public string StaffEmailAddress { get; set; }
    }
}
