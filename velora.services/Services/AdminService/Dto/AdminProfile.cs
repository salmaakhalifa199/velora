using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Data;
using velora.core.Entities;
using velora.core.Entities.IdentityEntities;
using velora.core.Entities.OrderEntities;
using velora.services.Services.AuthService.Dto;
using velora.services.Services.ProductService.Dto;

namespace velora.services.Services.AdminService.Dto
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<Person, AuthResponseDto>();

            CreateMap<Person, UserManagementDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
                .ForMember(dest => dest.ProductBrand, opt => opt.Ignore())
                .ForMember(dest => dest.ProductCategory, opt => opt.Ignore()); ;

            CreateMap<Order, OrderSummaryDto>()
                .ForMember(dest => dest.BuyerEmail, opt => opt.MapFrom(src => src.BuyerEmail))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Subtotal)) 
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Status));

            CreateMap<OrderItem, ProductSalesDto>()
               .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ItemOrdered.ProductId))  
               .ForMember(dest => dest.TotalQuantity, opt => opt.MapFrom(src => src.Quantity));
        }
    }
}
