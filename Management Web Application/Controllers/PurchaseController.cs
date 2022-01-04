using AutoMapper;
using Management_Web_Application.DomainModel;
using Management_Web_Application.Models.PaymentModels;
using Management_Web_Application.Models.PurchaseModels;
using Management_Web_Application.Services.GetPurchaseRequestService;
using Management_Web_Application.Services.ProductService;
using Management_Web_Application.Services.PurchaseService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly ISendPurchaseRequestService _sendPurchaseService;
        private readonly IGetPurchaseRequestService _getPurchaseService;
        private readonly IProductService _productService;
        private IMapper _mapper;
        private readonly string _baseURL = Environment.GetEnvironmentVariable("BASE_URL");

        public PurchaseController(ISendPurchaseRequestService sendPurchaseService, IGetPurchaseRequestService getPurchaseRequestService, IProductService productService,IMapper mapper)
        {
            _sendPurchaseService = sendPurchaseService;
            _getPurchaseService = getPurchaseRequestService;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseReadViewModel>>> Index()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            try
            {
                var getAllPurcahse = await _getPurchaseService.GetAllPurchaseAsync(accessToken);
                return View(_mapper.Map<IEnumerable<PurchaseReadViewModel>>(getAllPurcahse));
            }
            catch
            {
                return Redirect($"{baseURL}home/noaction");
            }
        }

        [HttpGet]
        public async Task<ActionResult<SendPaymentModel>> Payment(int ID, SendPaymentModel paymentModel)
        {
            try
            {
                paymentModel.ID = ID;
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var getPurchaseRequest = await _getPurchaseService.GetPurchaseRequestByIdAsync(ID, accessToken);
                paymentModel.ProductName = getPurchaseRequest.name;
                paymentModel.ProductQty = getPurchaseRequest.quantity;
                paymentModel.ProductPrice = getPurchaseRequest.totalPrice;
                paymentModel.ProductDesc = getPurchaseRequest.description;
                ModelState.Clear();
                return View(paymentModel);
            }
            catch
            {
                return Redirect($"{_baseURL}home/noaction");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SendPaymentModel>> Payment(SendPaymentModel paymentModel)
        {
            try
            {
                var status = 2;
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var getPurchaseRequest = await _getPurchaseService.GetPurchaseRequestByIdAsync(paymentModel.ID, accessToken);
                ModelState.Clear();
                SendPurchaseRequestDomainModel sendPurchaseRequestDomainModel = new SendPurchaseRequestDomainModel();
                sendPurchaseRequestDomainModel.AccountName = paymentModel.PaymentAccountName;
                sendPurchaseRequestDomainModel.CardNumber = paymentModel.PaymentCardNumber;
                sendPurchaseRequestDomainModel.ProductId = getPurchaseRequest.productId;
                sendPurchaseRequestDomainModel.Quantity = getPurchaseRequest.quantity;
                await _sendPurchaseService.SendPurchaseRequest(sendPurchaseRequestDomainModel);
                await _getPurchaseService.UpdatePurchaseRequestStatus(getPurchaseRequest, accessToken, status);
                return View();
            }
            catch (Exception e)
            {
                if(e.InnerException.Message.Contains("No connection could be made because the target machine actively refused it."))
                {
                    return Redirect($"{_baseURL}Purchase/NoConnection");
                }
                return Redirect($"{_baseURL}Purchase");
            }
        }

        public async Task<ActionResult> SendPurchaseRequest(int ID, PurchaseSendViewModel purchaseReadViewModel)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            try 
            {
                // Status 2 = Approved
                var status = 2;
                
                // Getting the purchase request information from the model
                var getPurchaseRequest = await _getPurchaseService.GetPurchaseRequestByIdAsync(ID, accessToken);
                
                // Sending the purchase request to the Third Party Stock Service
                //await _sendPurchaseService.SendPurchaseRequest(purchaseRequest);
                
                // Sending the updated status of the purchase request to the Purchase Service
                //await _getPurchaseService.UpdatePurchaseRequestStatus(getPurchaseRequest, accessToken, status);

                // Sending the new amount of stock to the Product Service
                var updateProductQtyDomainModel = new UpdateProductQtyDomainModel();
                updateProductQtyDomainModel.productQuantityToAdd = getPurchaseRequest.quantity;
                await _productService.UpdateProductQty(updateProductQtyDomainModel, ID, accessToken);

                return Redirect($"{baseURL}purchase/Index?test");
            }
            catch (Exception e)
            {
                return Redirect($"{baseURL}purchase/Index");
            }
        }
        public async Task<ActionResult> DenyPurchaseRequest(int ID, PurchaseSendViewModel purchaseReadViewModel)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            try
            {
                // Status 3 = Denied
                var status = 3;

                // Getting the purchase request information from the model
                var getPurchaseRequest = await _getPurchaseService.GetPurchaseRequestByIdAsync(ID, accessToken);

                await _getPurchaseService.UpdatePurchaseRequestStatus(getPurchaseRequest, accessToken, status);
                //await _sendPurchaseService.SendPurchaseRequest(purchaseRequest);
                return Redirect($"{baseURL}purchase/Index?test");

            }
            catch
            {
                return Redirect($"{baseURL}purchase/Index");
            }
        }

        public IActionResult NoConnection()
        {
            return View();
        }
    }
}
