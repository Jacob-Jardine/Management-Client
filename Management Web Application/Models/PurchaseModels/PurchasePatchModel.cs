using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Models.PurchaseModels
{
    public class PurchasePatchModel
    {
        public string op { get; set; }

        public string path { get; set; }

        public int value { get; set; }
    }
}
