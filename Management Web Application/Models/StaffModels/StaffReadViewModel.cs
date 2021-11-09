using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Models
{
    public class StaffReadViewModel
    {
        public int StaffID { get; set; }
        public string StaffFirstName { get; set; }
        public string StaffLastName { get; set; }
        public string StaffEmailAddress { get; set; }
        public string StaffFullName { get { return string.Format("{0} {1}", StaffFirstName, StaffLastName); } }
    }
}
