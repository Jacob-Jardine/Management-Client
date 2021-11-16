using Management_Web_Application.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Services.PurchaseService
{
    public class FakeSendPurchaseRequestService : ISendPurchaseRequestService
    {
        private readonly List<SendPurchaseRequestDomainModel> _purchaseList;

        public FakeSendPurchaseRequestService()
        {
            _purchaseList = new List<SendPurchaseRequestDomainModel>() { };
        }
        public Task SendPurchaseRequest(SendPurchaseRequestDomainModel purchaseDomainModel)
        {
            _purchaseList.Add(purchaseDomainModel);
            return Task.CompletedTask;
        }
    }
}
