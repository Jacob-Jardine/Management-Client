using Management_Web_Application.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Services.PurchaseService
{
    public class FakeThirdPartyStockService : IThirdPartyStockService
    {
        private readonly List<SendPurchaseRequestDomainModel> _purchaseList;

        public FakeThirdPartyStockService()
        {
            _purchaseList = new List<SendPurchaseRequestDomainModel>() { };
        }
        public async Task<bool> SendPurchaseRequest(SendPurchaseRequestDomainModel purchaseDomainModel)
        {
            try
            {
                _purchaseList.Add(purchaseDomainModel);
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}
