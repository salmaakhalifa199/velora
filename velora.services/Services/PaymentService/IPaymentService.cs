
using velora.services.Services.CartService.Dto;
using velora.services.Services.OrderService.Dto;

namespace velora.services.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<CustomerCartDto> CreateOrUpdatePaymentIntent(CustomerCartDto input);
        Task<OrderDto> UpdateOrderPaymentSucceeded(string paymentIntentId);
        Task<OrderDto> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}
