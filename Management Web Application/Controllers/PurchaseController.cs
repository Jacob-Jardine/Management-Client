using AutoMapper;
using Management_Web_Application.DomainModel;
using Management_Web_Application.Models.PaymentModels;
using Management_Web_Application.Models.PurchaseModels;
using Management_Web_Application.Services.GetPurchaseRequestService;
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
        private IMapper _mapper;
        private readonly string _baseURL = Environment.GetEnvironmentVariable("BASE_URL");

        public PurchaseController(ISendPurchaseRequestService sendPurchaseService, IGetPurchaseRequestService getPurchaseRequestService, IMapper mapper)
        {
            _sendPurchaseService = sendPurchaseService;
            _getPurchaseService = getPurchaseRequestService;
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
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var getPurchaseRequest = await _getPurchaseService.GetPurchaseRequestByIdAsync(ID, accessToken);
                paymentModel.ProductName = getPurchaseRequest.name;
                paymentModel.ProductQty = getPurchaseRequest.quantity;
                paymentModel.ProductPrice = getPurchaseRequest.totalPrice;
                paymentModel.ProductDesc = getPurchaseRequest.description;
                return View(paymentModel);
            }
            catch
            {
                return Redirect($"{_baseURL}home/noaction");
            }
        }

        public async Task<ActionResult> SendPurchaseRequest(int ID, PurchaseSendViewModel purchaseReadViewModel)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            try 
            {
                var status = 2;
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
        public async Task<ActionResult> DenyPurchaseRequest(int ID, PurchaseSendViewModel purchaseReadViewModel)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            try
            {
                var status = 3;
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
    }
}
