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
                new GetPurchaseRequestDomainModel() {quantity = 1, name = "Screen Protector", purchaseRequestStatus = 1},
                new GetPurchaseRequestDomainModel() {quantity = 2, name = "Phone", purchaseRequestStatus = 1}
            };
        }

        public Task DeletePurchaseRequest(int ID)
        {
            _purchaseList.RemoveAll(x => x.purchaseRequestID == ID);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<GetPurchaseRequestDomainModel>> GetAllPurchaseAsync(string token) => Task.FromResult(_purchaseList.AsEnumerable());

        public Task<GetPurchaseRequestDomainModel> GetPurchaseRequestByIdAsync(int? ID, string token) => Task.FromResult(_purchaseList.FirstOrDefault(x => x.purchaseRequestID == ID));

        public async Task UpdatePurchaseRequestStatus(GetPurchaseRequestDomainModel purchaseRequestDomainModel, string token, int status)
        {
            if(status == 2)
            {
                var purchaseRequest = _purchaseList.FirstOrDefault(x => x.purchaseRequestID == purchaseRequestDomainModel.purchaseRequestID);
                purchaseRequest.purchaseRequestStatus = 2;
            }
            else if( status == 3)
            {
                var purchaseRequest = _purchaseList.FirstOrDefault(x => x.purchaseRequestID == purchaseRequestDomainModel.purchaseRequestID);
                purchaseRequest.purchaseRequestStatus = 3;
            }
        }
    }
}
