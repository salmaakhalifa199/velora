using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.OrderEntities;
using velora.services.Services.OrderService.Dto;

namespace velora.services.Services.OrderService.Dto
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderDto, Order>();
            CreateMap<AddressDto, ShippingAddress>().ReverseMap();

            CreateMap<Order, OrderDto>()
               .ForMember(dest => dest.DeliveryMethod, options => options.MapFrom(src => src.DeliveryMethod.ShortName))
               .ForMember(dest => dest.ShippingPrice, options => options.MapFrom(src => src.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
               .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.ItemOrdered.ProductId))
               .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.ItemOrdered.ProductName))
               .ForMember(dest => dest.PictureUrl, options => options.MapFrom(src => src.ItemOrdered.PictureUrl))
               .ForMember(dest => dest.PictureUrl, options => options.MapFrom<OrderItemPictureUrlResolver>()).ReverseMap();

            CreateMap<DeliveryMethods, DeliveryMethodDto>().ReverseMap(); 
        }
    }
}
 