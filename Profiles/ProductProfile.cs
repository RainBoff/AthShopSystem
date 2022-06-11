using asp_mvc_std_v2.Models;
using AutoMapper;
using Data_Model_P;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_mvc_std_v2.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Produkt, ProductViewModel>();
        }
    }
}
