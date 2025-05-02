using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.OrderEntities;

namespace velora.services.Services.OrderService.Dto
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl) || source.ItemOrdered.PictureUrl.Contains(source.ItemOrdered.PictureUrl))
            {
                return $"{_configuration["BaseUrl"]}{source.ItemOrdered.PictureUrl}";
            }
            return null;
        }
    }
}
