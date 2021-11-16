using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Models.PurchaseModels
{
    public class PurchaseSendViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string AccountName { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime When { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductEan { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
    }
}
