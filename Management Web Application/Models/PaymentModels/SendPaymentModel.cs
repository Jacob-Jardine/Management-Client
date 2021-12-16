using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Models.PaymentModels
{
    public class SendPaymentModel
    {
        public int ID { get; set; }
        public string ProductName { get; set; }

        public int ProductQty { get; set; }

        public string ProductDesc { get; set; }

        public decimal ProductPrice { get; set; }

        [Display(Name = "Payment Account Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter A Name")]
        [StringLength(24, ErrorMessage = "Name can't be empty")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string PaymentAccountName { get; set; }

        [Display(Name = "Card Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Card Number")]
        [StringLength(16, ErrorMessage = "Card Number can't be empty")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers only")]
        public string PaymentCardNumber { get; set; }

        [Display(Name = "Exp Month")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Exp Month")]
        [StringLength(2, ErrorMessage = "Exp Month can't be empty")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers only")]
        public string PaymentCardExpMonth { get; set; }

        [Display(Name = "Exp Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Exp Year")]
        [StringLength(2, ErrorMessage = "Exp Year can't be empty")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers only")]
        public string PaymentCardExpYear { get; set; }
        
        [Display(Name = "CVV")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter CVV")]
        [StringLength(3, ErrorMessage = "CVV can't be empty")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numbers only")]
        public string PaymentCardCVV { get; set; }
    }
}
