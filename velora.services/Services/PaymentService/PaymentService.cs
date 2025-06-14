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
namespace velora.services.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitWork _unitWork;
        private readonly ICartService _cartService;

        public PaymentService(IConfiguration configuration, IUnitWork unitWork, ICartService cartService)
        {
            _configuration = configuration;
            _unitWork = unitWork;
            _cartService = cartService;
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
            var orders = (await _unitWork.Repository<Order, Guid>().GetAllAsync())
        .Where(o => o.PaymentIntentId == paymentIntentId)
        .ToList();

            if (orders.Count == 0)
            {
                // No matching order
                return null;
            }

            if (orders.Count > 1)
            {
                // ❌ Duplicate problem: you can log a warning, or handle it as needed
                throw new InvalidOperationException($"Multiple orders found with PaymentIntentId {paymentIntentId}");
            }

            var order = orders.First(); // ✅ Now safe

            order.Status = OrderStatus.Placed; // or whatever your enum is
            order.UpdatedAt = DateTime.UtcNow;
            _unitWork.Repository<Order, Guid>().Update(order);
            await _unitWork.CompleteAsync();

            return order;
        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            if (string.IsNullOrEmpty(paymentIntentId))
                throw new Exception("Invalid PaymentIntentId");

            var orders = await _unitWork.Repository<Order, Guid>().GetAllAsync();
            var order = orders.FirstOrDefault(o => o.PaymentIntentId == paymentIntentId);

            if (order == null)
                throw new Exception("Order not found");

            order.OrderPaymentStatus = OrderPaymentStatus.Failed;
            await _unitWork.CompleteAsync();
            return order;
        }
   
    }
}


