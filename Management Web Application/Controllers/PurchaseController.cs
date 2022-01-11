using AutoMapper;
using Management_Web_Application.DomainModel;
using Management_Web_Application.Models.PaymentModels;
using Management_Web_Application.Models.ProductModels;
using Management_Web_Application.Models.PurchaseModels;
using Management_Web_Application.Services.GetPurchaseRequestService;
using Management_Web_Application.Services.ProductService;
using Management_Web_Application.Services.PurchaseService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Management_Web_Application.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IThirdPartyStockService _sendPurchaseService;
        private readonly IGetPurchaseRequestService _getPurchaseService;
        private readonly IProductService _productService;
        private IMapper _mapper;
        private readonly string _baseURL = Environment.GetEnvironmentVariable("BASE_URL");

        public PurchaseController(IThirdPartyStockService sendPurchaseService, IGetPurchaseRequestService getPurchaseRequestService, IProductService productService,IMapper mapper)
        {
            _sendPurchaseService = sendPurchaseService;
            _getPurchaseService = getPurchaseRequestService;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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

                var updateProductQtyDomainModel = new UpdateProductQtyDomainModel();
                updateProductQtyDomainModel.productQuantityToAdd = getPurchaseRequest.quantity;
                var id = _productService.GetProducts(accessToken);
                bool flag = true;
                foreach (var item in id.Result)
                {
                    if (item.productName == getPurchaseRequest.name)
                    {
                        await _productService.UpdateProductQty(updateProductQtyDomainModel, item.productID, accessToken);
                        flag = false;
                    }
                }
                if(flag == true)
                {
                    var postProd = new PostToProductServiceDomainModel();
                    postProd.productQuantity = sendPurchaseRequestDomainModel.Quantity;
                    postProd.productPrice = getPurchaseRequest.price;
                    postProd.productName = getPurchaseRequest.name;
                    postProd.productDescription = getPurchaseRequest.description;
                    await _productService.PostProduct(postProd, accessToken);
                }
                return Redirect($"{_baseURL}Purchase");
            }
            catch (Exception e)
            {
                try
                {
                    if (e.InnerException.Message.Contains("No connection could be made because the target machine actively refused it."))
                    {
                        return Redirect($"{_baseURL}Purchase/NoConnection");
                    }
                }
                catch
                {
                    return Redirect($"{_baseURL}Purchase");
                }
                return Redirect($"{_baseURL}Purchase");
            }
        }
        [Authorize]
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

        [Authorize]
        public IActionResult NoConnection()
        {
            return View();
        }
    }
}
