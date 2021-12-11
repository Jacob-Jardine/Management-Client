using Management_Web_Application.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Models.StaffModels
{
    public class StaffPermissionsViewModel
    {
        public List<ReadAuth0PermissionsDomainModel> existingPermissionList { get; set; }
        public int StaffID { get; set; }
        [Display(Name = "Customer Account Deleltion Request")]
        public bool CustomerAccountDeleltionRequestBool { get; set; }
        [Display(Name = "Create An Order")]
        public bool OrderBool { get; set; }
        [Display(Name = "Add A Product")]
        public bool ProductBool { get; set; }
        [Display(Name = "Create A Product Review")]
        public bool ProductReviewBool { get; set; }
        [Display(Name = "Create A Purchase Request")]
        public bool PurchaseReqiestBool { get; set; }
        [Display(Name = "Create Staff Members")]
        public bool StaffBool { get; set; }
        [Display(Name = "Send Requests To Thrid Party Services")]
        public bool ThirdPartyStockRequestBool { get; set; }
        [Display(Name = "Delete Products")]
        public bool DeleteProductsBool { get; set; }
        [Display(Name = "Delete Staff Members")]
        public bool DeleteStaffBool { get; set; }
        [Display(Name = "Confirm Invoices")]
        public bool ConfirmInvoiceBool { get; set; }
        [Display(Name = "Update A Customer Acount Deletion Request")]
        public bool UpdateCustomerAccountDeleltionRequestBool { get; set; }
        [Display(Name = "Update Orders")]
        public bool UpdateOrdersBool { get; set; }
        [Display(Name = "Update Products")]
        public bool UpdateProductsBool { get; set; }
        [Display(Name = "Update A Product Review")]
        public bool UpdateProductReviewBool { get; set; }
        [Display(Name = "Update Purchase Requests Status")]
        public bool UpdatePurchaseRequestStatusBool { get; set; }
        [Display(Name = "Update Staff Members")]
        public bool UpdateStaffBool { get; set; }
        [Display(Name = "Read A Customer Acount Deletion Request")]
        public bool ReadCustomerAccountDeleltionRequetBool { get; set; }
        [Display(Name = "Read A Customer Acount Deletion Requests")]
        public bool ReadCustomerAccountDeleltionRequestsBool { get; set; }
        [Display(Name = "Read Customer Specific Orders")]
        public bool ReadCustomerOrdersBool { get; set; }
        [Display(Name = "Read Specific Invoiced Order")]
        public bool ReadInvoicedOrderBool { get; set; }
        [Display(Name = "Read All Invoiced Orders")]
        public bool ReadInvoicedOrdersBool { get; set; }
        [Display(Name = "Read All Orders")]
        public bool ReadOrdersBool { get; set; }
        [Display(Name = "Read All Pending Invoices")]
        public bool ReadPendingInvoicesBool { get; set; }
        [Display(Name = "Read All Pending Purchase Requests")]
        public bool ReadPendingPurchaseRequests { get; set; }
        [Display(Name = "Read Product Detials")]
        public bool ReadProductDetailsBool { get; set; }
        [Display(Name = "Read Specific Product Review")]
        public bool ReadProductReviewBool { get; set; }
        [Display(Name = "Read All Product Reviews")]
        public bool ReadProductReviewsBool{ get; set; }
        [Display(Name = "Read All Products")]
        public bool ReadProdcutsBool { get; set; }
        [Display(Name = "Read Specific Purchase Request")]
        public bool ReadPurchaseRequestBool { get; set; }
        [Display(Name = "Read All Purchase Requests")]
        public bool ReadPurchaseRequestsBool { get; set; }
        [Display(Name = "Read Specific Staff Member")]
        public bool ReadStaffMemberBool { get; set; }
        [Display(Name = "Read All Staff Members")]
        public bool ReadStaffMembersBool { get; set; }
        [Display(Name = "Read All Third Party Stock")]
        public bool ReadThirdPartyStockBool { get; set; }
        [Display(Name = "Read Visible Product Reviews")]
        public bool ReadVisibleProductReviewsBool { get; set; }
    }
}
