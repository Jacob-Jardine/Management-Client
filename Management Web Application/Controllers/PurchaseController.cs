using AutoMapper;
using Management_Web_Application.Models.PurchaseModels;
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
        private readonly IPurchaseRequestService _purchaseService;
        private IMapper _mapper;

        public PurchaseController(IPurchaseRequestService purchaseService, IMapper mapper)
        {
            _purchaseService = purchaseService;
            _mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<PurchaseReadViewModel>>> Index()
        {
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            try
            {
                var getAllPurcahse = await _purchaseService.GetAllPurchaseAsync();
                return View(_mapper.Map<IEnumerable<PurchaseReadViewModel>>(getAllPurcahse));
            }
            catch
            {
                return Redirect($"{baseURL}home/noaction");
            }
        }

        public async Task<ActionResult> ConfirmPurchaseRequest(int? ID)
        {
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            try
            {
                await _purchaseService.SendPurchaseRequest(ID);
                return Redirect($"{baseURL}purchase/Index");
            }
            catch
            {
                return Redirect($"{baseURL}home/noaction");
            }
        }

        public async Task<ActionResult> DenyPurchaseRequest(int? ID)
        {
            string baseURL = Environment.GetEnvironmentVariable("BASE_URL");
            try
            {
                await _purchaseService.DenyOrder(ID);
                return Redirect($"{baseURL}purchase/Index");
            }
            catch
            {
                return Redirect($"{baseURL}home/noaction");
            }
        }
    }
}
