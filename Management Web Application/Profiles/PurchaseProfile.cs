using AutoMapper;
using Management_Web_Application.DomainModel;
using Management_Web_Application.Models.PurchaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Profiles
{
    public class PurchaseProfile : Profile
    {
        public PurchaseProfile() 
        {
            CreateMap<GetPurchaseRequestDomainModel, PurchaseReadViewModel>();
            CreateMap<PurchaseReadViewModel, GetPurchaseRequestDomainModel>();
            CreateMap<PurchaseSendViewModel, SendPurchaseRequestDomainModel>();
            CreateMap<SendPurchaseRequestDomainModel, PurchaseSendViewModel>();
        }
    }
}
