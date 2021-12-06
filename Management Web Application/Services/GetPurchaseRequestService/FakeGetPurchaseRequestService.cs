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
                //new GetPurchaseRequestDomainModel() {Id = 2, AccountName = "GROUP A Test", CardNumber = "GROUP A Test", ProductId = 1, Quantity = 1, When = DateTime.Now, ProductName = "sample string 3", ProductEan = "sample string 4", TotalPrice = 1.1M}
            };
        }

        public Task DeletePurchaseRequest(int ID)
        {
            _purchaseList.RemoveAll(x => x.purchaseRequestID == ID);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<GetPurchaseRequestDomainModel>> GetAllPurchaseAsync(string token) => Task.FromResult(_purchaseList.AsEnumerable());

        public Task<GetPurchaseRequestDomainModel> GetPurchaseRequestByIdAsync(int? ID, string token) => Task.FromResult(_purchaseList.FirstOrDefault(x => x.purchaseRequestID == ID));

        public Task<GetPurchaseRequestDomainModel> UpdatePurchaseRequestStatus(GetPurchaseRequestDomainModel purchaseRequestDomainModel, string token)
        {
            throw new NotImplementedException();
        }
    }
}
