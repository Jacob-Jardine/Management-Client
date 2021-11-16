using Management_Web_Application.DomainModel;
using Management_Web_Application.Services.PurchaseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Services.GetPurchaseRequestService
{
    public class FakeGetPurchaseRequestService : IGetPurchaseRequestService
    {
        private readonly List<GetPurchaseRequestDomainModel> _purchaseList;

        public FakeGetPurchaseRequestService()
        {
            _purchaseList = new List<GetPurchaseRequestDomainModel>()
            {
                new GetPurchaseRequestDomainModel() {Id = 1, AccountName = "sample string 1", CardNumber = "sample string 2", ProductId = 1, Quantity = 1, When = DateTime.Now, ProductName = "sample string 3", ProductEan = "sample string 4", TotalPrice = 1.1M}
            };
        }

        public Task<IEnumerable<GetPurchaseRequestDomainModel>> GetAllPurchaseAsync() => Task.FromResult(_purchaseList.AsEnumerable());
    }
}
