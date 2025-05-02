using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using velora.core.Entities.OrderEntities;
using velora.services.Services.OrderService.Dto;

namespace velora.services.Services.OrderService
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderDto orderDto);
        Task<IReadOnlyList<OrderDto>> GetAllOrdersForUserAsync(string buyerEmail);
        Task<OrderDto> GetOrderByIdAsync(Guid id, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethods>> GetAllDeliveryMethodsAsync();
        Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    }
}
