using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Models.PurchaseModels
{
    public class PurchaseReadViewModel
    {
        public int purchaseRequestID { get; set; }
        public string accountName { get; set; }
        public string cardNumber { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public DateTime when { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int productEan { get; set; }
        public int brandId { get; set; }
        public string brandName { get; set; }
        public decimal price { get; set; }
        public decimal totalPrice { get; set; }
        public int purchaseRequestStatus { get; set; }
    }
}
