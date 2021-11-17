﻿using AutoMapper;
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
        private readonly ISendPurchaseRequestService _sendPurchaseService;
        private readonly IGetPurchaseRequestService _getPurchaseService;
        private IMapper _mapper;

        public PurchaseController(ISendPurchaseRequestService sendPurchaseService, IGetPurchaseRequestService getPurchaseRequestService, IMapper mapper)
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
                var getPurchaseRequest = await _getPurchaseService.GetPurchaseRequestByIdAsync(purchaseReadViewModel.Id);
                purchaseReadViewModel = _mapper.Map<PurchaseSendViewModel>(getPurchaseRequest);
                var test = _mapper.Map<SendPurchaseRequestDomainModel>(purchaseReadViewModel);
                await _sendPurchaseService.SendPurchaseRequest(test);
                await _getPurchaseService.DeletePurchaseRequest(purchaseReadViewModel.Id);
                return Redirect($"{baseURL}purchase/Index?test");
            }
            catch 
            {
                return Redirect($"{baseURL}purchase/Index");
            }
        }
    }
}
