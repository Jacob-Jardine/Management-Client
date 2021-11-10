using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Models.PurchaseModels
{
    public class PurchaseReadViewModel
    {
        public int PurchaseID { get; set; }
        public string PurchaseName { get; set; }
        public int PurchaseQTY { get; set; }
        public double PurchaseCost { get; set; }
    }
}
