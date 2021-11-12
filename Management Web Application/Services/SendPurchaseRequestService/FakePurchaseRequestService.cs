using Management_Web_Application.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Services.PurchaseService
{
    public class FakePurchaseRequestService : IPurchaseRequestService
    {
        private readonly List<PurchaseDomainModel> _purchaseList;

        public FakePurchaseRequestService()
        {
            _purchaseList = new List<PurchaseDomainModel>() { };
        }
        public Task SendPurchaseRequest(PurchaseDomainModel purchaseDomainModel)
        {
            int newPurchaseRequestID = 2;
            purchaseDomainModel.PurchaseID = newPurchaseRequestID;
            _purchaseList.Add(purchaseDomainModel);
            return Task.CompletedTask;
        }
    }
}
