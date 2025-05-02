using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.repository.Cart.Models;

namespace velora.services.Services.CartService.Dto
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CustomerCartDto, CustomerCart>().ReverseMap();
            CreateMap<CartItemDto, CartItem>().ReverseMap();
        }
    }
}
