using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.DomainModel
{
    public class GetPurchaseRequestDomainModel
    {
        [Required]
        public int purchaseRequestID { get; set; }
        [Required]
        public string accountName { get; set; }
        [Required]
        public string cardNumber { get; set; }
        [Required]
        public int productId { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public DateTime when { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public string productEan { get; set; }
        [Required]
        public int brandId { get; set; }
        [Required]
        public string brandName { get; set; }
        [Required]
        public decimal price { get; set; }
        [Required]
        public decimal totalPrice { get; set; }
        [Required]
        public int purchaseRequestStatus { get; set; }
    }
}
