using AutoMapper;
using Store.services.Services.ProductService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Data;
using velora.core.Entities;

namespace velora.services.Services.ProductService.Dto
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<Product, ProductDto>()
                    .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                    .ForMember(dest => dest.ProductCategory, opt => opt.MapFrom(src => src.ProductCategory.Name))
                    .ForMember(dest => dest.PictureUrl, options => options.MapFrom<ProductPictureUrlResolver>())
                    .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
                    .ReverseMap(); 



            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.ProductBrand, opt => opt.Ignore())
                .ForMember(dest => dest.ProductCategory, opt => opt.Ignore());
        }
    }
}
