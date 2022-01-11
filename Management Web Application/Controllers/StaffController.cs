using AutoMapper;
using AutoMapper.Configuration;
using Management_Web_Application.DomainModel;
using Management_Web_Application.Models;
using Management_Web_Application.Models.StaffModels;
using Management_Web_Application.Services.Auth0Service;
using Management_Web_Application.Services.StaffService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Management_Web_Application.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly IAuth0Service _auth0Service;
        private IMapper _mapper;
        private readonly string _baseURL = Environment.GetEnvironmentVariable("BASE_URL");

        public StaffController(IStaffService staffService, IAuth0Service auth0Service, IMapper mapper)
        {
            _staffService = staffService;
            _mapper = mapper;
            _auth0Service = auth0Service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult<IEnumerable<StaffReadViewModel>>> GetAllStaff()
        {
            try
            {
                //var accessToken = await HttpContext.GetTokenAsync("access_token");
                var accessToken = await GetAuthToken(HttpContext, "access_token");
                //var accessToken = await HttpContext.GetTokenAsync("access_token");
                var getAllStaff = await _staffService.GetAllStaffAsync(accessToken);
                return View(_mapper.Map<IEnumerable<StaffReadViewModel>>(getAllStaff));
            }
            catch (Exception e)
            {
                return Redirect($"{_baseURL}home/noaction");
            }
        }

        public async Task<ActionResult<StaffReadViewModel>> GetStaffByID(int ID)
        {
            try
            {
                var accessToken = await GetAuthToken(HttpContext, "access_token");
                var getStaffByID = await _staffService.GetStaffByIDAsnyc(ID, accessToken);
                if (getStaffByID != null)
                {
                    return View(_mapper.Map<StaffReadViewModel>(getStaffByID));
                }
                else
                {
                    return Redirect($"{_baseURL}home/noaction");
                }
            }
            catch (Exception e)
            {
                return Redirect($"{_baseURL}home/noaction");
            }
        }

        [HttpGet]
        public IActionResult CreateStaff()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateStaff(StaffCreateViewModel staffCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(staffCreateViewModel);
            }
            try
            {
                staffCreateViewModel.StaffEmailAddress = staffCreateViewModel.StaffEmailAddress.ToLower();
                var accessToken = await GetAuthToken(HttpContext, "access_token");
                var auth0DomainModel = new CreateAuth0UserDomainModel()
                {
                    given_name = staffCreateViewModel.StaffFirstName,
                    family_name = staffCreateViewModel.StaffLastName,
                    email = staffCreateViewModel.StaffEmailAddress
                };
                var staffModel = _mapper.Map<StaffDTO>(staffCreateViewModel);
                StaffDTO newStaffDomainModel = await _staffService.CreateStaffAsync(staffModel, accessToken);
                await _auth0Service.CreateAuth0User(auth0DomainModel);
                Thread.Sleep(500);
                addBasicRolesToUser(staffCreateViewModel.StaffEmailAddress);
                return Redirect($"{_baseURL}staff/GetStaffById/{newStaffDomainModel.StaffID}");
            }
            catch (Exception e)
            {
                return Redirect($"{_baseURL}home/noaction");
            }
        }

        private async void addBasicRolesToUser(string staffEmailAddress)
        {
            try
            {
                var permissionList = new AddAuth0PermissionsDomainModels
                {
                    permissions = new List<AddAuth0PermissionsDomainModel>()

                };
                var getAuth0User = await _auth0Service.SearchByEmail(staffEmailAddress);
                var test = getAuth0User.First().user_id;

                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:customer_account_deletion_request" });
                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:order" });
                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:product" });
                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:purchase-request" });
                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:product_review" });
                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:staff" });
                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:tps_request" });
                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "delete:product" });
                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "delete:staff" });
                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:confirm_invoice" });
                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:customer_account_deletion_requests" });
                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:order" });
                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:product" });
                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:product_review" });
                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:purchase-request" });
                //permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:staff" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:customer_account_deletion_request" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:customer_account_deletion_requests" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:customer_orders" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:invoiced_order" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:invoiced_orders" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:order" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:pending_invoices" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:pending-purchase-requests" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:product" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:product_review" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:product_reviews" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:products" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:purchase-request" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:purchase-requests" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:staff" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:staffs" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:tps_stock" });
                permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:visible_product_reviews" });

                var send = await _auth0Service.UpdateAuth0UserPermissions(permissionList, test);
            }
            catch (Exception e)
            {

            }
        }

        [HttpGet]
        public async Task<ActionResult> UpdateStaff(int ID, StaffUpdateViewModel staffUpdateViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(staffUpdateViewModel);
                }
                var accessToken = await GetAuthToken(HttpContext, "access_token");
                var getStaffByID = await _staffService.GetStaffByIDAsnyc(ID, accessToken);
                //var getAuth0User = await _auth0Service.SearchByEmail(getStaffByID.StaffEmailAddress);
                if (getStaffByID != null)
                {
                    return View(_mapper.Map<StaffUpdateViewModel>(getStaffByID));
                }
                else
                {
                    return Redirect($"{_baseURL}home/noaction");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Redirect($"{_baseURL}home/noaction");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateStaff(StaffUpdateViewModel staffUpdateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(staffUpdateViewModel);
            }
            try
            {
                var accessToken = await GetAuthToken(HttpContext, "access_token");
                var staffModel = _mapper.Map<StaffUpdateDTO>(staffUpdateViewModel);
                if(await _staffService.UpdateStaff(staffModel, accessToken) == true)
                {
                    return Redirect($"{_baseURL}staff/GetStaffById/{staffUpdateViewModel.StaffID}");
                }
                else
                {
                    return Redirect($"{_baseURL}home/noaction");
                }
            }
            catch (Exception e)
            {
                return Redirect($"{_baseURL}home/noaction");
            }
        }

        public async Task<ActionResult> DeleteStaff(int ID)
        {
            try
            {
                var accessToken = await GetAuthToken(HttpContext, "access_token");
                var getStaffByID = await _staffService.GetStaffByIDAsnyc(ID, accessToken);
                if (getStaffByID != null)
                {
                    await _staffService.DeleteStaff(getStaffByID.StaffID, accessToken);
                    var getAuth0User = await _auth0Service.SearchByEmail(getStaffByID.StaffEmailAddress);
                    var test = getAuth0User.First().user_id;
                    await _auth0Service.DeleteAuth0User(test);
                    return Redirect($"{_baseURL}staff/GetAllStaff");
                }
                else
                {
                    return Redirect($"{_baseURL}home/noaction");
                }
            }
            catch (Exception e)
            {
                return Redirect($"{_baseURL}home/noaction");
            }
        }

        [HttpGet]
        public async Task<ActionResult> UpdatePermissions(int ID, StaffPermissionsViewModel staffUpdateViewModel)
        {
            try
            {
                staffUpdateViewModel.StaffID = ID;
                if (!ModelState.IsValid)
                {
                    return View(staffUpdateViewModel);
                }
                var accessToken = await GetAuthToken(HttpContext, "access_token");
                var getStaffByID = await _staffService.GetStaffByIDAsnyc(ID, accessToken);
                var getAuth0User = await _auth0Service.SearchByEmail(getStaffByID.StaffEmailAddress);
                var test = getAuth0User.First().user_id;
                var send = await _auth0Service.ReadAuth0Permissions(test);
                staffUpdateViewModel.existingPermissionList = send.ToList();
                foreach (var item in staffUpdateViewModel.existingPermissionList)
                {
                    if (item.permission_name.Equals("add:customer_account_deletion_request"))
                    {
                        staffUpdateViewModel.CustomerAccountDeleltionRequestBool = true;
                    }
                    if (item.permission_name.Equals("add:order"))
                    {
                        staffUpdateViewModel.OrderBool = true;
                    }
                    if (item.permission_name.Equals("add:product"))
                    {
                        staffUpdateViewModel.ProductBool = true;
                    }
                    if (item.permission_name.Equals("add:purchase-request"))
                    {
                        staffUpdateViewModel.PurchaseReqiestBool = true;
                    }
                    if (item.permission_name.Equals("add:product_review"))
                    {
                        staffUpdateViewModel.ProductReviewBool = true;
                    }
                    if (item.permission_name.Equals("add:staff"))
                    {
                        staffUpdateViewModel.StaffBool = true;
                    }
                    if (item.permission_name.Equals("add:tps_request"))
                    {
                        staffUpdateViewModel.ThirdPartyStockRequestBool = true;
                    }
                    if (item.permission_name.Equals("delete:product"))
                    {
                        staffUpdateViewModel.DeleteProductsBool = true;
                    }
                    if (item.permission_name.Equals("delete:staff"))
                    {
                        staffUpdateViewModel.DeleteStaffBool = true;
                    }
                    if (item.permission_name.Equals("edit:confirm_invoice"))
                    {
                        staffUpdateViewModel.ConfirmInvoiceBool = true;
                    }
                    if (item.permission_name.Equals("edit:customer_account_deletion_requests"))
                    {
                        staffUpdateViewModel.UpdateCustomerAccountDeleltionRequestBool = true;
                    }
                    if (item.permission_name.Equals("edit:order"))
                    {
                        staffUpdateViewModel.UpdateOrdersBool = true;
                    }
                    if (item.permission_name.Equals("edit:product"))
                    {
                        staffUpdateViewModel.UpdateProductsBool = true;
                    }
                    if (item.permission_name.Equals("edit:product_review"))
                    {
                        staffUpdateViewModel.UpdateProductReviewBool = true;
                    }
                    if (item.permission_name.Equals("edit:purchase-request"))
                    {
                        staffUpdateViewModel.UpdatePurchaseRequestStatusBool = true;
                    }
                    if (item.permission_name.Equals("edit:staff"))
                    {
                        staffUpdateViewModel.UpdateStaffBool = true;
                    }
                    if (item.permission_name.Equals("read:customer_account_deletion_request"))
                    {
                        staffUpdateViewModel.ReadCustomerAccountDeleltionRequetBool = true;
                    }
                    if (item.permission_name.Equals("read:customer_account_deletion_requests"))
                    {
                        staffUpdateViewModel.ReadCustomerAccountDeleltionRequestsBool = true;
                    }
                    if (item.permission_name.Equals("read:customer_orders"))
                    {
                        staffUpdateViewModel.ReadCustomerOrdersBool = true;
                    }
                    if (item.permission_name.Equals("read:invoiced_order"))
                    {
                        staffUpdateViewModel.ReadInvoicedOrderBool = true;
                    }
                    if (item.permission_name.Equals("read:invoiced_orders"))
                    {
                        staffUpdateViewModel.ReadInvoicedOrdersBool = true;
                    }
                    if (item.permission_name.Equals("read:order"))
                    {
                        staffUpdateViewModel.ReadOrdersBool = true;
                    }
                    if (item.permission_name.Equals("read:pending_invoices"))
                    {
                        staffUpdateViewModel.ReadPendingInvoicesBool = true;
                    }
                    if (item.permission_name.Equals("read:pending-purchase-requests"))
                    {
                        staffUpdateViewModel.ReadPendingPurchaseRequests = true;
                    }
                    if (item.permission_name.Equals("read:product"))
                    {
                        staffUpdateViewModel.ReadProductDetailsBool = true;
                    }
                    if (item.permission_name.Equals("read:product_review"))
                    {
                        staffUpdateViewModel.ReadProductReviewBool = true;
                    }
                    if (item.permission_name.Equals("read:product_reviews"))
                    {
                        staffUpdateViewModel.ReadProductReviewsBool = true;
                    }
                    if (item.permission_name.Equals("read:purchase-requests"))
                    {
                        staffUpdateViewModel.ReadPurchaseRequestBool = true;
                    }
                    if (item.permission_name.Equals("read:products"))
                    {
                        staffUpdateViewModel.ReadProdcutsBool = true;
                    }
                    if (item.permission_name.Equals("read:purchase-request"))
                    {
                        staffUpdateViewModel.ReadPurchaseRequestBool = true;
                    }
                    if (item.permission_name.Equals("read:purchase-requests"))
                    {
                        staffUpdateViewModel.ReadPurchaseRequestsBool = true;
                    }
                    if (item.permission_name.Equals("read:purchase-requests"))
                    {
                        staffUpdateViewModel.ReadPurchaseRequestsBool = true;
                    }
                    if (item.permission_name.Equals("read:staff"))
                    {
                        staffUpdateViewModel.ReadStaffMemberBool = true;
                    }
                    if (item.permission_name.Equals("read:staffs"))
                    {
                        staffUpdateViewModel.ReadStaffMembersBool = true;
                    }
                    if (item.permission_name.Equals("read:tps_stock"))
                    {
                        staffUpdateViewModel.ReadThirdPartyStockBool = true;
                    }
                    if (item.permission_name.Equals("read:visible_product_reviews"))
                    {
                        staffUpdateViewModel.ReadVisibleProductReviewsBool = true;
                    }
                    if (item.permission_name.Equals("read:product_price_history"))
                    {
                        staffUpdateViewModel.ReadProductPriceHistory = true;
                    }
                }
            }
            catch
            {

            }
            return View(staffUpdateViewModel);


            //try
            //{
            //    if (!ModelState.IsValid) 
            //    {
            //        return View(staffUpdateViewModel);
            //    }
            //    var getStaffByID = await _staffService.GetStaffByIDAsnyc(ID);
            //    var getAuth0User = await _auth0Service.SearchByEmail(getStaffByID.StaffEmailAddress);
            //    if (getStaffByID != null)
            //    {
            //        return View(_mapper.Map<StaffUpdateViewModel>(getStaffByID));
            //    }
            //    else
            //    {
            //        return Redirect($"{_baseURL}home/noaction");
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //   return Redirect($"{_baseURL}home/noaction");
            //}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePermissions(StaffPermissionsViewModel staffUpdateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(staffUpdateViewModel);
            }
            try
            {
                var permissionList = new AddAuth0PermissionsDomainModels
                {
                    permissions = new List<AddAuth0PermissionsDomainModel>()

                };
                if (staffUpdateViewModel.CustomerAccountDeleltionRequestBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:customer_account_deletion_request" });
                }
                if (staffUpdateViewModel.OrderBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:order" });
                }
                if (staffUpdateViewModel.ProductBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:product" });
                }
                if (staffUpdateViewModel.PurchaseReqiestBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:purchase-request" });
                }
                if (staffUpdateViewModel.ProductReviewBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:product_review" });
                }
                if (staffUpdateViewModel.StaffBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:staff" });
                }
                if (staffUpdateViewModel.ThirdPartyStockRequestBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:tps_request" });
                }
                if (staffUpdateViewModel.DeleteProductsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "delete:product" });
                }
                if (staffUpdateViewModel.DeleteStaffBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "delete:staff" });
                }
                if (staffUpdateViewModel.ConfirmInvoiceBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:confirm_invoice" });
                }
                if (staffUpdateViewModel.UpdateCustomerAccountDeleltionRequestBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:customer_account_deletion_requests" });
                }
                if (staffUpdateViewModel.UpdateOrdersBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:order" });
                }
                if (staffUpdateViewModel.UpdateProductsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:product" });
                }
                if (staffUpdateViewModel.UpdateProductReviewBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:product_review" });
                }
                if (staffUpdateViewModel.UpdatePurchaseRequestStatusBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:purchase-request" });
                }
                if (staffUpdateViewModel.UpdateStaffBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:staff" });
                }
                if (staffUpdateViewModel.ReadCustomerAccountDeleltionRequetBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:customer_account_deletion_request" });
                }
                if (staffUpdateViewModel.ReadCustomerAccountDeleltionRequestsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:customer_account_deletion_requests" });
                }
                if (staffUpdateViewModel.ReadCustomerOrdersBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:customer_orders" });
                }
                if (staffUpdateViewModel.ReadInvoicedOrderBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:invoiced_order" });
                }
                if (staffUpdateViewModel.ReadInvoicedOrdersBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:invoiced_orders" });
                }
                if (staffUpdateViewModel.ReadOrdersBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:order" });
                }
                if (staffUpdateViewModel.ReadPendingInvoicesBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:pending_invoices" });
                }
                if (staffUpdateViewModel.ReadPendingPurchaseRequests == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:pending-purchase-requests" });
                }
                if (staffUpdateViewModel.ReadProductDetailsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:product" });
                }
                if (staffUpdateViewModel.ReadProductReviewBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:product_review" });
                }
                if (staffUpdateViewModel.ReadProductReviewsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:product_reviews" });
                }
                if (staffUpdateViewModel.ReadProdcutsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:products" });
                }
                if (staffUpdateViewModel.ReadPurchaseRequestBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:purchase-request" });
                }
                if (staffUpdateViewModel.ReadPurchaseRequestsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:purchase-requests" });
                }
                if (staffUpdateViewModel.ReadStaffMemberBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:staff" });
                }
                if (staffUpdateViewModel.ReadStaffMembersBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:staffs" });
                }
                if (staffUpdateViewModel.ReadThirdPartyStockBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:tps_stock" });
                }
                if (staffUpdateViewModel.ReadVisibleProductReviewsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:visible_product_reviews" });
                }
                if (staffUpdateViewModel.ReadAllPriceHistoryOfAllProducts == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:product_prices" });
                }
                if (staffUpdateViewModel.ReadProductPriceHistory == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:product_price_history" });
                }
                var accessToken = await GetAuthToken(HttpContext, "access_token");
                var getStaffByID = await _staffService.GetStaffByIDAsnyc(staffUpdateViewModel.StaffID, accessToken);
                var getAuth0User = await _auth0Service.SearchByEmail(getStaffByID.StaffEmailAddress);
                var test = getAuth0User.First().user_id;
                var send = await _auth0Service.ReadAuth0Permissions(test);
                staffUpdateViewModel.existingPermissionList = send.ToList();

                foreach(var item in permissionList.permissions)
                {
                    var x = new ReadAuth0PermissionsDomainModel();
                    var q = item.permission_name.ToString();
                    x.permission_name = q;
                    staffUpdateViewModel.existingPermissionList.Add(x);
                }


                var update = await _auth0Service.UpdateAuth0UserPermissions(permissionList, test);

                return Redirect($"{_baseURL}staff/GetStaffById/{staffUpdateViewModel.StaffID}");
            }
            catch
            {
                return Redirect($"{_baseURL}home/noaction");
            }
        }

        [HttpGet]
        public async Task<ActionResult> RemovePermissons(int ID, StaffPermissionsViewModel staffUpdateViewModel)
        {
            try
            {
                staffUpdateViewModel.StaffID = ID;
                if (!ModelState.IsValid)
                {
                    return View(staffUpdateViewModel);
                }
                var accessToken = await GetAuthToken(HttpContext, "access_token");
                var getStaffByID = await _staffService.GetStaffByIDAsnyc(ID, accessToken);
                var getAuth0User = await _auth0Service.SearchByEmail(getStaffByID.StaffEmailAddress);
                var test = getAuth0User.First().user_id;
                var send = await _auth0Service.ReadAuth0Permissions(test);
                staffUpdateViewModel.existingPermissionList = send.ToList();

                staffUpdateViewModel.CustomerAccountDeleltionRequestBool = true;
                staffUpdateViewModel.OrderBool = true;
                staffUpdateViewModel.ProductBool = true;
                staffUpdateViewModel.PurchaseReqiestBool = true;
                staffUpdateViewModel.ProductReviewBool = true;
                staffUpdateViewModel.StaffBool = true;
                staffUpdateViewModel.ThirdPartyStockRequestBool = true;
                staffUpdateViewModel.DeleteProductsBool = true;
                staffUpdateViewModel.DeleteStaffBool = true;
                staffUpdateViewModel.ConfirmInvoiceBool = true;
                staffUpdateViewModel.UpdateCustomerAccountDeleltionRequestBool = true;
                staffUpdateViewModel.UpdateOrdersBool = true;
                staffUpdateViewModel.UpdateProductsBool = true;
                staffUpdateViewModel.UpdateProductReviewBool = true;
                staffUpdateViewModel.UpdatePurchaseRequestStatusBool = true;
                staffUpdateViewModel.UpdateStaffBool = true;
                staffUpdateViewModel.ReadCustomerAccountDeleltionRequetBool = true;
                staffUpdateViewModel.ReadCustomerAccountDeleltionRequestsBool = true;
                staffUpdateViewModel.ReadCustomerOrdersBool = true;
                staffUpdateViewModel.ReadInvoicedOrderBool = true;
                staffUpdateViewModel.ReadInvoicedOrdersBool = true;
                staffUpdateViewModel.ReadOrdersBool = true;
                staffUpdateViewModel.ReadPendingInvoicesBool = true;
                staffUpdateViewModel.ReadPendingPurchaseRequests = true;
                staffUpdateViewModel.ReadProductDetailsBool = true;
                staffUpdateViewModel.ReadProductReviewBool = true;
                staffUpdateViewModel.ReadProductReviewsBool = true;
                staffUpdateViewModel.ReadPurchaseRequestBool = true;
                staffUpdateViewModel.ReadProdcutsBool = true;
                staffUpdateViewModel.ReadPurchaseRequestBool = true;
                staffUpdateViewModel.ReadPurchaseRequestsBool = true;
                staffUpdateViewModel.ReadPurchaseRequestsBool = true;
                staffUpdateViewModel.ReadStaffMemberBool = true;
                staffUpdateViewModel.ReadStaffMembersBool = true;
                staffUpdateViewModel.ReadThirdPartyStockBool = true;
                staffUpdateViewModel.ReadVisibleProductReviewsBool = true;

                foreach (var item in staffUpdateViewModel.existingPermissionList)
                {
                    if (item.permission_name.Equals("add:customer_account_deletion_request"))
                    {
                        staffUpdateViewModel.CustomerAccountDeleltionRequestBool = false;
                    }
                    if (item.permission_name.Equals("add:order"))
                    {
                        staffUpdateViewModel.OrderBool = false;
                    }
                    if (item.permission_name.Equals("add:product"))
                    {
                        staffUpdateViewModel.ProductBool = false;
                    }
                    if (item.permission_name.Equals("add:purchase-request"))
                    {
                        staffUpdateViewModel.PurchaseReqiestBool = false;
                    }
                    if (item.permission_name.Equals("add:product_review"))
                    {
                        staffUpdateViewModel.ProductReviewBool = false;
                    }
                    if (item.permission_name.Equals("add:staff"))
                    {
                        staffUpdateViewModel.StaffBool = false;
                    }
                    if (item.permission_name.Equals("add:tps_request"))
                    {
                        staffUpdateViewModel.ThirdPartyStockRequestBool = false;
                    }
                    if (item.permission_name.Equals("delete:product"))
                    {
                        staffUpdateViewModel.DeleteProductsBool = false;
                    }
                    if (item.permission_name.Equals("delete:staff"))
                    {
                        staffUpdateViewModel.DeleteStaffBool = false;
                    }
                    if (item.permission_name.Equals("edit:confirm_invoice"))
                    {
                        staffUpdateViewModel.ConfirmInvoiceBool = false;
                    }
                    if (item.permission_name.Equals("edit:customer_account_deletion_requests"))
                    {
                        staffUpdateViewModel.UpdateCustomerAccountDeleltionRequestBool = false;
                    }
                    if (item.permission_name.Equals("edit:order"))
                    {
                        staffUpdateViewModel.UpdateOrdersBool = false;
                    }
                    if (item.permission_name.Equals("edit:product"))
                    {
                        staffUpdateViewModel.UpdateProductsBool = false;
                    }
                    if (item.permission_name.Equals("edit:product_review"))
                    {
                        staffUpdateViewModel.UpdateProductReviewBool = false;
                    }
                    if (item.permission_name.Equals("edit:purchase-request"))
                    {
                        staffUpdateViewModel.UpdatePurchaseRequestStatusBool = false;
                    }
                    if (item.permission_name.Equals("edit:staff"))
                    {
                        staffUpdateViewModel.UpdateStaffBool = false;
                    }
                    if (item.permission_name.Equals("read:customer_account_deletion_request"))
                    {
                        staffUpdateViewModel.ReadCustomerAccountDeleltionRequetBool = false;
                    }
                    if (item.permission_name.Equals("read:customer_account_deletion_requests"))
                    {
                        staffUpdateViewModel.ReadCustomerAccountDeleltionRequestsBool = false;
                    }
                    if (item.permission_name.Equals("read:customer_orders"))
                    {
                        staffUpdateViewModel.ReadCustomerOrdersBool = false;
                    }
                    if (item.permission_name.Equals("read:invoiced_order"))
                    {
                        staffUpdateViewModel.ReadInvoicedOrderBool = false;
                    }
                    if (item.permission_name.Equals("read:invoiced_orders"))
                    {
                        staffUpdateViewModel.ReadInvoicedOrdersBool = false;
                    }
                    if (item.permission_name.Equals("read:order"))
                    {
                        staffUpdateViewModel.ReadOrdersBool = false;
                    }
                    if (item.permission_name.Equals("read:pending_invoices"))
                    {
                        staffUpdateViewModel.ReadPendingInvoicesBool = false;
                    }
                    if (item.permission_name.Equals("read:pending-purchase-requests"))
                    {
                        staffUpdateViewModel.ReadPendingPurchaseRequests = false;
                    }
                    if (item.permission_name.Equals("read:product"))
                    {
                        staffUpdateViewModel.ReadProductDetailsBool = false;
                    }
                    if (item.permission_name.Equals("read:product_review"))
                    {
                        staffUpdateViewModel.ReadProductReviewBool = false;
                    }
                    if (item.permission_name.Equals("read:product_reviews"))
                    {
                        staffUpdateViewModel.ReadProductReviewsBool = false;
                    }
                    if (item.permission_name.Equals("read:purchase-requests"))
                    {
                        staffUpdateViewModel.ReadPurchaseRequestBool = false;
                    }
                    if (item.permission_name.Equals("read:products"))
                    {
                        staffUpdateViewModel.ReadProdcutsBool = false;
                    }
                    if (item.permission_name.Equals("read:purchase-request"))
                    {
                        staffUpdateViewModel.ReadPurchaseRequestBool = false;
                    }
                    if (item.permission_name.Equals("read:purchase-requests"))
                    {
                        staffUpdateViewModel.ReadPurchaseRequestsBool = false;
                    }
                    if (item.permission_name.Equals("read:purchase-requests"))
                    {
                        staffUpdateViewModel.ReadPurchaseRequestsBool = false;
                    }
                    if (item.permission_name.Equals("read:staff"))
                    {
                        staffUpdateViewModel.ReadStaffMemberBool = false;
                    }
                    if (item.permission_name.Equals("read:staffs"))
                    {
                        staffUpdateViewModel.ReadStaffMembersBool = false;
                    }
                    if (item.permission_name.Equals("read:tps_stock"))
                    {
                        staffUpdateViewModel.ReadThirdPartyStockBool = false;
                    }
                    if (item.permission_name.Equals("read:visible_product_reviews"))
                    {
                        staffUpdateViewModel.ReadVisibleProductReviewsBool = false;
                    }
                    if (item.permission_name.Equals("read:product_prices"))
                    {
                        staffUpdateViewModel.ReadAllPriceHistoryOfAllProducts = false;
                    }
                    if (item.permission_name.Equals("read:product_price_history"))
                    {
                        staffUpdateViewModel.ReadProductPriceHistory = false;
                    }
                }
                
            }
            catch
            {

            }
            
            return View(staffUpdateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePermissons(StaffPermissionsViewModel staffUpdateViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(staffUpdateViewModel);
            }
            try
            {
                var permissionList = new AddAuth0PermissionsDomainModels
                {
                    permissions = new List<AddAuth0PermissionsDomainModel>()

                };
                if (staffUpdateViewModel.CustomerAccountDeleltionRequestBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:customer_account_deletion_request" });
                }
                if (staffUpdateViewModel.OrderBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:order" });
                }
                if (staffUpdateViewModel.ProductBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:product" });
                }
                if (staffUpdateViewModel.PurchaseReqiestBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:purchase-request" });
                }
                if (staffUpdateViewModel.ProductReviewBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:product_review" });
                }
                if (staffUpdateViewModel.StaffBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:staff" });
                }
                if (staffUpdateViewModel.ThirdPartyStockRequestBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "add:tps_request" });
                }
                if (staffUpdateViewModel.DeleteProductsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "delete:product" });
                }
                if (staffUpdateViewModel.DeleteStaffBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "delete:staff" });
                }
                if (staffUpdateViewModel.ConfirmInvoiceBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:confirm_invoice" });
                }
                if (staffUpdateViewModel.UpdateCustomerAccountDeleltionRequestBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:customer_account_deletion_requests" });
                }
                if (staffUpdateViewModel.UpdateOrdersBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:order" });
                }
                if (staffUpdateViewModel.UpdateProductsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:product" });
                }
                if (staffUpdateViewModel.UpdateProductReviewBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:product_review" });
                }
                if (staffUpdateViewModel.UpdatePurchaseRequestStatusBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:purchase-request" });
                }
                if (staffUpdateViewModel.UpdateStaffBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "edit:staff" });
                }
                if (staffUpdateViewModel.ReadCustomerAccountDeleltionRequetBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:customer_account_deletion_request" });
                }
                if (staffUpdateViewModel.ReadCustomerAccountDeleltionRequestsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:customer_account_deletion_requests" });
                }
                if (staffUpdateViewModel.ReadCustomerOrdersBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:customer_orders" });
                }
                if (staffUpdateViewModel.ReadInvoicedOrderBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:invoiced_order" });
                }
                if (staffUpdateViewModel.ReadInvoicedOrdersBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:invoiced_orders" });
                }
                if (staffUpdateViewModel.ReadOrdersBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:order" });
                }
                if (staffUpdateViewModel.ReadPendingInvoicesBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:pending_invoices" });
                }
                if (staffUpdateViewModel.ReadPendingPurchaseRequests == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:pending-purchase-requests" });
                }
                if (staffUpdateViewModel.ReadProductDetailsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:product" });
                }
                if (staffUpdateViewModel.ReadProductReviewBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:product_review" });
                }
                if (staffUpdateViewModel.ReadProductReviewsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:product_reviews" });
                }
                if (staffUpdateViewModel.ReadProdcutsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:products" });
                }
                if (staffUpdateViewModel.ReadPurchaseRequestBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:purchase-request" });
                }
                if (staffUpdateViewModel.ReadPurchaseRequestsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:purchase-requests" });
                }
                if (staffUpdateViewModel.ReadStaffMemberBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:staff" });
                }
                if (staffUpdateViewModel.ReadStaffMembersBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:staffs" });
                }
                if (staffUpdateViewModel.ReadThirdPartyStockBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:tps_stock" });
                }
                if (staffUpdateViewModel.ReadVisibleProductReviewsBool == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:visible_product_reviews" });
                }
                if (staffUpdateViewModel.ReadAllPriceHistoryOfAllProducts == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:product_prices" });
                }
                if (staffUpdateViewModel.ReadProductPriceHistory == true)
                {
                    permissionList.permissions.Add(new AddAuth0PermissionsDomainModel() { permission_name = "read:product_price_history" });
                }
                var accessToken = await GetAuthToken(HttpContext, "access_token");
                var getStaffByID = await _staffService.GetStaffByIDAsnyc(staffUpdateViewModel.StaffID, accessToken);
                var getAuth0User = await _auth0Service.SearchByEmail(getStaffByID.StaffEmailAddress);
                var test = getAuth0User.First().user_id;
                var send = await _auth0Service.RemoveAuth0Permissions(permissionList, test);
                return Redirect($"{_baseURL}staff/GetStaffById/{staffUpdateViewModel.StaffID}");
            }
            catch
            {
                return Redirect($"{_baseURL}home/noaction");
            }
        }
        public async static Task<string> GetAuthToken(HttpContext context, string key)
        {
            string token = context.Session.GetString(key); if (String.IsNullOrEmpty(token))
            {
                token = await context.GetTokenAsync(key);
                context.Session.SetString(key, token);
            }
            return token;
        }
    }
}
