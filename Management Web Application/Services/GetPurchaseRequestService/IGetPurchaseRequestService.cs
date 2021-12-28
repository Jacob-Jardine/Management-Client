using Management_Web_Application.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Services.GetPurchaseRequestService
{
    public interface IGetPurchaseRequestService
    {
        public Task<IEnumerable<GetPurchaseRequestDomainModel>> GetAllPurchaseAsync(string token);
        public Task<GetPurchaseRequestDomainModel> GetPurchaseRequestByIdAsync(int? ID, string token);
        public Task UpdatePurchaseRequestStatus(GetPurchaseRequestDomainModel purchaseRequestDomainModel, string token, int status);
    }
}
