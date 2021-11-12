using AutoMapper;
using Management_Web_Application.DomainModel;
using Management_Web_Application.Models.PurchaseModels;
using Management_Web_Application.Services.GetPurchaseRequestService;
using Management_Web_Application.Services.PurchaseService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IPurchaseRequestService _sendPurchaseService;
        private readonly IGetPurchaseRequestService _getPurchaseService;
        private IMapper _mapper;

        public PurchaseController(IPurchaseRequestService sendPurchaseService, IGetPurchaseRequestService getPurchaseRequestService, IMapper mapper)
        {
            _sendPurchaseService = sendPurchaseService;
            _getPurchaseService = getPurchaseRequestService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseReadViewModel>>> Index()
        {
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            try
            {
                var getAllPurcahse = await _getPurchaseService.GetAllPurchaseAsync();
                return View(_mapper.Map<IEnumerable<PurchaseReadViewModel>>(getAllPurcahse));
            }
            catch
            {
                return Redirect($"{baseURL}home/noaction");
            }
        }

        public async Task<ActionResult> SendPurchaseRequest(int? ID, PurchaseSendViewModel purchaseReadViewModel)
        {
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            try 
            {
                var getAllPurcahse = await _getPurchaseService.GetAllPurchaseAsync();
                var getPurchaseRequest = getAllPurcahse.FirstOrDefault(x => x.PurchaseID == ID);
                purchaseReadViewModel = _mapper.Map<PurchaseSendViewModel>(getPurchaseRequest);
                var test = _mapper.Map<PurchaseDomainModel>(purchaseReadViewModel);
                await _sendPurchaseService.SendPurchaseRequest(test);
                return Redirect($"{baseURL}staff/GetAllStaff");
            }
            catch 
            {
                return Redirect($"{baseURL}purchase/Index");
            }
        }
    }
}
