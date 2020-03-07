using AutoMapper;
using HyphenProject.Business.ObjectDtos;
using HyphenProject.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HyphenProject.Business.Mappings
{
  public   class CustomMappingProfile:Profile
    {
        public CustomMappingProfile()
        {
            CreateMap<Product, ProductInputDto>();
            CreateMap<ProductInputDto, Product>();
        }
       
         
    }
}
