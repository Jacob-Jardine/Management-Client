using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.DomainModel
{
    public class SendPurchaseRequestDomainModel
    {
        [Required]
        public int purchaseRequestID { get; set; }
        [Required]
        public int purchaseRequestStatus { get; set; }
    }
}
