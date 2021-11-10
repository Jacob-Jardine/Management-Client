using Management_Web_Application.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Services.PurchaseService
{
    public class FakePurchaseService : IPurchaseService
    {
        private readonly List<PurchaseDomainModel> _purchaseList;

        public FakePurchaseService()
        {
            _purchaseList = new List<PurchaseDomainModel>()
            {
                new PurchaseDomainModel() {PurchaseID = 1, PurchaseQTY = 1, PurchaseName = "Jeans", PurchaseCost = 1.1},
                new PurchaseDomainModel() {PurchaseID = 2, PurchaseQTY = 2, PurchaseName = "T-Shirt", PurchaseCost = 22.22},
                new PurchaseDomainModel() {PurchaseID = 3, PurchaseQTY = 3, PurchaseName = "Trainers", PurchaseCost = 333.33},
                new PurchaseDomainModel() {PurchaseID = 4, PurchaseQTY = 4, PurchaseName = "Caps", PurchaseCost = 4444.44},
                new PurchaseDomainModel() {PurchaseID = 5, PurchaseQTY = 5, PurchaseName = "Belts", PurchaseCost = 555.55}
            };
        }
        public Task<IEnumerable<PurchaseDomainModel>> GetAllPurchaseAsync() => Task.FromResult(_purchaseList.AsEnumerable());
    }
}
