//using AutoMapper;
//using Microsoft.Extensions.Configuration;
//using Store.Repository.Interfaces;
//using Stripe;
//using velora.core.Entities.OrderEntities;
//using Product = velora.core.Entities.Product;
//using velora.repository.Specifications.OrderSpecs;
//using velora.services.Services.CartService;
//using velora.services.Services.CartService.Dto;
//using velora.services.Services.OrderService.Dto;
//namespace velora.services.Services.PaymentService
//{
//    public class PaymentService : IPaymentService
//    {
//        private readonly IConfiguration _configuration;
//        private readonly IUnitWork _unitWork;
//        private readonly ICartService _CartService;
//        private readonly IMapper _mapper;

//        public PaymentService(IConfiguration configuration, IUnitWork unitWork, ICartService cartService, IMapper mapper)
//        {
//            _configuration = configuration;
//            _unitWork = unitWork;
//            _CartService = cartService;
//            _mapper = mapper;
//        }
//        public async Task<CustomerCartDto> CreateOrUpdatePaymentIntent(CustomerCartDto cart)
//        {
//            StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];

//            if (cart is null)
//                throw new Exception("Basket is Empty");


//            var deliveryMethod = await _unitWork.Repository<DeliveryMethods, int>().GetByIdAsync(cart.DeliveryMethodId.Value);

//            if (deliveryMethod is null)
//                throw new Exception("Delivery Method Not Provided");

//            decimal shippingPrice = deliveryMethod.Price;

//            foreach (var item in cart.CartItems)
//            {
//                var product = await _unitWork.Repository<Product, int>().GetByIdAsync(item.ProductId);

//                if (item.Price != product.Price)
//                {
//                    item.Price = product.Price;
//                }
//            }

//            var service = new PaymentIntentService();

//            PaymentIntent paymentIntent;

//            if (string.IsNullOrEmpty(cart.PaymentIntentId))
//            {
//                var options = new PaymentIntentCreateOptions
//                {
//                    Amount = (long)cart.CartItems.Sum(item => item.Quantity * (item.Price * 100)) + (long)(shippingPrice * 100),
//                    Currency = "usd",
//                    PaymentMethodTypes = new List<string> { "card" }

//                };

//                paymentIntent = await service.CreateAsync(options);

//                cart.PaymentIntentId = paymentIntent.Id;
//                cart.ClientSecret = paymentIntent.ClientSecret;

//            }
//            else
//            {
//                var options = new PaymentIntentUpdateOptions
//                {
//                    Amount = (long)cart.CartItems.Sum(item => item.Quantity * (item.Price * 100)) + (long)(shippingPrice * 100),
//                };

//                await service.UpdateAsync(cart.PaymentIntentId, options);
//            }
//            await _CartService.UpdateCartAsync(cart);

//            return cart;

//        }

//        public async Task<OrderDto> UpdateOrderPaymentFailed(string paymentIntentId)
//            {
//                var specs = new OrderWithPaymentIntentSpecification(paymentIntentId);

//                var order = await _unitWork.Repository<Order, Guid>().GetByIdWithSpecAsync(specs);

//                if (order is null)
//                    throw new Exception(" order does not exist");

//                order.OrderPaymentStatus = OrderPaymentStatus.Failed;

//                _unitWork.Repository<Order, Guid>().Update(order);

//                await _unitWork.CompleteAsync();

//                var mapperOrder = _mapper.Map<OrderDto>(order);

//                return mapperOrder;

//            }

//            public async Task<OrderDto> UpdateOrderPaymentSucceeded(string paymentIntentId)
//            {
//                var specs = new OrderWithPaymentIntentSpecification(paymentIntentId);

//                var order = await _unitWork.Repository<Order, Guid>().GetByIdWithSpecAsync(specs);

//                if (order is null)
//                    throw new Exception(" order does not exist");

//                order.OrderPaymentStatus = OrderPaymentStatus.Received;

//                _unitWork.Repository<Order, Guid>().Update(order);

//                await _unitWork.CompleteAsync();

//                await _CartService.DeleteCartAsync(order.CartId);

//                var mapperOrder = _mapper.Map<OrderDto>(order);

//                return mapperOrder;
//            }
//    }
// }

