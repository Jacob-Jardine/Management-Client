using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.DomainModel
{
    public class PostToProductServiceDomainModel
    {
        [Required]
        public int productQuantity { get; set; }
        [Required]
        public decimal productPrice { get; set; }
        [Required]
        public string productName { get; set; }
        [Required]
        public string productDescription { get; set; }
    }
}
