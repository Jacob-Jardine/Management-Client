using Management_Web_Application.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Services.PurchaseService
{
    public interface IPurchaseService
    {
        public Task<IEnumerable<PurchaseDomainModel>> GetAllPurchaseAsync();
        public Task ConfirmOrder(int? ID);
        public Task DenyOrder(int? ID);
    }
}
