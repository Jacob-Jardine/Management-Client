using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.DomainModel
{
    public class PurchaseDomainModel
    {
        [Required]
        public int PurchaseID { get; set; }
        [Required]
        public string PurchaseName { get; set; }
        [Required]
        public int PurchaseQTY { get; set; }
        [Required]
        public double PurchaseCost { get; set; }
    }
}
