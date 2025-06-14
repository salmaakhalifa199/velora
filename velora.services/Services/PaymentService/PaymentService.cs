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
using velora.Repository.Repositories;
using velora.repository.Cart.Models;
using Microsoft.Extensions.Logging;
namespace velora.services.Services.PaymentService
{

    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitWork _unitWork;
        private readonly ICartService _cartService;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IConfiguration configuration, IUnitWork unitWork, ICartService cartService, ILogger<PaymentService> logger)
        {
            _configuration = configuration;
            _unitWork = unitWork;
            _cartService = cartService;
            _logger = logger;
        }

        public async Task<CustomerCartDto> CreateOrUpdatePaymentIntent(CustomerCartDto cart)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];

            // 💡 Step 0: Check if an order with this CartId & PaymentIntentId already exists
            var existingOrder = (await _unitWork.Repository<Order, Guid>().GetAllAsync())
                .FirstOrDefault(o => o.CartId == cart.Id && o.PaymentIntentId == cart.PaymentIntentId);

            if (existingOrder != null)
            {
                // 🛑 Stop if the order already exists — prevents duplicate orders
                return cart;
            }

            // 🧾 Step 1: Calculate total amount
            decimal total = cart.CartItems.Sum(item => item.Quantity * item.Price);

            // 💳 Step 2: Create or update payment intent
            PaymentIntent intent;
            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(total * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                var service = new PaymentIntentService();
                intent = await service.CreateAsync(options);

                cart.PaymentIntentId = intent.Id;
                cart.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)(total * 100)
                };

                var service = new PaymentIntentService();
                await service.UpdateAsync(cart.PaymentIntentId, options);
            }

            return cart;
        }



        public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            if (string.IsNullOrEmpty(paymentIntentId))
                throw new Exception("Invalid paymentIntentId");

            var order = await _unitWork.Repository<Order, Guid>()
                .GetByIdWithSpecAsync(new OrderWithPaymentIntentSpecification(paymentIntentId));

            if (order == null)
            {
                _logger.LogError($"Order not found for PaymentIntentId: {paymentIntentId}");
                throw new Exception($"Order not found for PaymentIntentId: {paymentIntentId}");
            }

            order.OrderPaymentStatus = OrderPaymentStatus.Received;
            order.Status = OrderStatus.Confirmed; // Better than 'Placed'
            order.UpdatedAt = DateTime.UtcNow;

            _unitWork.Repository<Order, Guid>().Update(order);
            await _unitWork.CompleteAsync();

            return order;
        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {

            var repo = _unitWork.Repository<Order, Guid>();

            var spec = new OrderWithPaymentIntentSpecification(paymentIntentId);
            var order = await repo.GetByIdWithSpecAsync(spec);

            if (order == null)
            {
                throw new Exception($"No order found for PaymentIntentId: {paymentIntentId}");
            }

            order.OrderPaymentStatus = OrderPaymentStatus.Failed;
            order.Status = OrderStatus.Cancelled;

            repo.Update(order);
            await _unitWork.CompleteAsync();

            return order;
        }
   
    }
}


