
using velora.core.Entities.OrderEntities;
using velora.repository.Cart.Models;
using velora.services.Services.CartService.Dto;
using velora.services.Services.OrderService.Dto;

namespace velora.services.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<CustomerCartDto> CreateOrUpdatePaymentIntent(CustomerCartDto input);
        Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId);
        Task<Order> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}
