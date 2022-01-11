using Management_Web_Application.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Services.PurchaseService
{
    /// <summary>
    /// Interface for third party stock service
    /// </summary>
    public interface IThirdPartyStockService
    {
        public Task<bool> SendPurchaseRequest(SendPurchaseRequestDomainModel purchaseDomainModel);
    }
}
