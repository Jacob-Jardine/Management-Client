using AutoMapper;
using Management_Web_Application.DomainModel;
using Management_Web_Application.Models.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<UpdateProductQtyDomainModel, UpdateProductQtyModel>();
            CreateMap<UpdateProductQtyModel, UpdateProductQtyDomainModel>();
        }
    }
}
