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
               .ForMember(dest => dest.ShippingPrice, options => options.MapFrom(src => src.DeliveryMethod.Price))
               .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Status))
               .ForMember(dest => dest.OrderPaymentStatus, opt => opt.MapFrom(src => src.OrderPaymentStatus))
               .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
               .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src =>
        (src.OrderItems != null ? src.OrderItems.Sum(i => i.Price * i.Quantity) : 0)
        + (src.DeliveryMethod != null ? src.DeliveryMethod.Price : 0)));
            CreateMap<OrderItem, OrderItemDto>()
               .ForMember(dest => dest.ProductId, options => options.MapFrom(src => src.ItemOrdered.ProductId))
               .ForMember(dest => dest.ProductName, options => options.MapFrom(src => src.ItemOrdered.ProductName))
               .ForMember(dest => dest.PictureUrl, options => options.MapFrom(src => src.ItemOrdered.PictureUrl))
               .ForMember(dest => dest.PictureUrl, options => options.MapFrom<OrderItemPictureUrlResolver>()).ReverseMap();

            CreateMap<DeliveryMethods, DeliveryMethodDto>().ReverseMap();
        }
    }
}
 