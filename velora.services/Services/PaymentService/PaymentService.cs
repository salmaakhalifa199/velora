using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Repository.Interfaces;
using Stripe;
using velora.core.Entities.OrderEntities;
using Product = velora.core.Entities.Product;
using velora.repository.Specifications.OrderSpecs;
using velora.services.Services.CartService;
using velora.services.Services.CartService.Dto;
using velora.services.Services.OrderService.Dto;
namespace velora.services.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitWork _unitWork;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public PaymentService(
            IConfiguration configuration,
            IUnitWork unitWork,
            ICartService cartService,
            IMapper mapper)
        {
            _configuration = configuration;
            _unitWork = unitWork;
            _cartService = cartService;
            _mapper = mapper;
        }

        public async Task<CustomerCartDto> CreateOrUpdatePaymentIntent(CustomerCartDto cart)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];

            if (cart == null)
                throw new Exception("Cart is empty");

            var deliveryMethod = await _unitWork.Repository<DeliveryMethods, int>().GetByIdAsync(cart.DeliveryMethodId.Value);
            if (deliveryMethod == null)
                throw new Exception("Delivery method not found");

            decimal shippingPrice = deliveryMethod.Price;

            foreach (var item in cart.CartItems)
            {
                var product = await _unitWork.Repository<Product, int>().GetByIdAsync(item.ProductId);
                if (item.Price != product.Price)
                {
                    item.Price = product.Price;
                }
            }

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;

            var totalAmount = (long)(cart.CartItems.Sum(i => i.Quantity * i.Price) * 100 + shippingPrice * 100);

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = totalAmount,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await service.CreateAsync(options);

                cart.PaymentIntentId = paymentIntent.Id;
                cart.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = totalAmount
                };
                await service.UpdateAsync(cart.PaymentIntentId, options);
            }

            await _cartService.UpdateCartAsync(cart);
            return cart;
        }

        public async Task<OrderDto> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            var spec = new OrderWithPaymentIntentSpecification(paymentIntentId);
            var order = await _unitWork.Repository<Order, Guid>().GetByIdWithSpecAsync(spec);
            if (order == null)
                throw new Exception("Order not found");

            order.OrderPaymentStatus = OrderPaymentStatus.Received;
            order.Status = OrderStatus.Placed;
            order.UpdatedAt = DateTime.UtcNow;

            _unitWork.Repository<Order, Guid>().Update(order);
            await _unitWork.CompleteAsync();
            await _cartService.DeleteCartAsync(order.CartId);

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var spec = new OrderWithPaymentIntentSpecification(paymentIntentId);
            var order = await _unitWork.Repository<Order, Guid>().GetByIdWithSpecAsync(spec);
            if (order == null)
                throw new Exception("Order not found");

            order.OrderPaymentStatus = OrderPaymentStatus.Failed;

            _unitWork.Repository<Order, Guid>().Update(order);
            await _unitWork.CompleteAsync();

            return _mapper.Map<OrderDto>(order);
        }
    }
}

