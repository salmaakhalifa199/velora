using AutoMapper;
using Store.Repository.Interfaces;
using velora.core.Data;
using Product = velora.core.Entities.Product;
using velora.core.Entities.OrderEntities;
using velora.repository.Specification.OrderSpecs;
using velora.services.Services.CartService;
using velora.services.Services.OrderService.Dto;
using Order = velora.core.Entities.OrderEntities.Order;
using velora.repository.Specifications.OrderSpecs;
using velora.services.Services.PaymentService;

namespace velora.services.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly ICartService _cartService;
        private readonly IUnitWork _unitWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public OrderService(ICartService cartService, IUnitWork unitWork, IMapper mapper, IPaymentService paymentService) 
        {
            _cartService = cartService;
            _unitWork = unitWork;
            _mapper = mapper;
            _paymentService = paymentService;
        }
        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto orderDto)
        {
            var cart = await _cartService.GetCartAsync(orderDto.CartId);
            if (cart is null)
                throw new Exception("Cart Not Exist");

            #region Fill order item list with items in the basket 
            var orderItems = new List<OrderItemDto>();

            foreach (var cartitem in cart.CartItems)
            {
                int productId = cartitem.ProductId;
                var productItem = await _unitWork.Repository<Product, int>().GetByIdAsync(productId);

                if (productItem is null)
                    throw new Exception($"Product With Id : {productId} Not Exist");

                var itemOrdered = new ProductItemOrdered
                {
                    ProductId = productItem.Id,
                    ProductName = productItem.Name,
                    PictureUrl = productItem.PictureUrl,
                };

                var orderItem = new OrderItem
                {
                    Price = productItem.Price,
                    Quantity = cartitem.Quantity,
                    ItemOrdered = itemOrdered,
                };

                var mapperOrderItem = _mapper.Map<OrderItemDto>(orderItem);
                orderItems.Add(mapperOrderItem);
            }
            #endregion

            #region Get Delivery Method
            var deliveryMethod = await _unitWork.Repository<DeliveryMethods, int>().GetByIdAsync(orderDto.DeliveryMethodId);
            if (deliveryMethod is null)
                throw new Exception("Delivery Method Not Provided");
            #endregion

            #region Calculate SubTotal
            var subTotal = orderItems.Sum(item => item.Quantity * item.Price);
            #endregion

            #region Check Existing Order by PaymentIntentId
            var specs = new OrderWithPaymentIntentSpecification(cart.PaymentIntentId);
            var existingOrder = await _unitWork.Repository<Order, Guid>().GetByIdWithSpecAsync(specs);

            if (existingOrder != null)
            {
                existingOrder.ShippingAddress = _mapper.Map<ShippingAddress>(orderDto.ShippingAddress);
                existingOrder.DeliveryMethodId = deliveryMethod.Id;
                existingOrder.Subtotal = subTotal;
                existingOrder.OrderItems = _mapper.Map<List<OrderItem>>(orderItems);

                _unitWork.Repository<Order, Guid>().Update(existingOrder);
                await _unitWork.CompleteAsync();
                return _mapper.Map<OrderDto>(existingOrder);
            }
            #endregion

            #region Create Payment Intent
            await _paymentService.CreateOrUpdatePaymentIntent(cart);
            #endregion

            #region Create New Order
            var order = new Order
            {
                DeliveryMethodId = deliveryMethod.Id,
                ShippingAddress = _mapper.Map<ShippingAddress>(orderDto.ShippingAddress),
                BuyerEmail = orderDto.BuyerEmail,
                UserId = orderDto.UserId,
                CartId = orderDto.CartId,
                OrderItems = _mapper.Map<List<OrderItem>>(orderItems),
                Subtotal = subTotal,
                PaymentIntentId = cart.PaymentIntentId
            };

            await _unitWork.Repository<Order, Guid>().AddAsync(order);
            await _unitWork.CompleteAsync();
            return _mapper.Map<OrderDto>(order);
            #endregion
        }


        public async Task<IReadOnlyList<DeliveryMethods>> GetAllDeliveryMethodsAsync()
        => await _unitWork.Repository<DeliveryMethods, int>().GetAllAsync();


        public async Task<IReadOnlyList<OrderDto>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var specs = new OrderWithItemSpecification(buyerEmail);

            var orders = await _unitWork.Repository<Order, Guid>().GetAllWithSpecAsync(specs);

            if (orders == null || orders.Count <= 0)
                throw new Exception("You do not have any orders yet!");

            var mappedOrders = _mapper.Map<List<OrderDto>>(orders);

            return mappedOrders;
        }


        public async Task<OrderDto> GetOrderByIdAsync(Guid id, string buyerEmail)
        {
            var specs = new OrderWithItemSpecification(id, buyerEmail);

            var order = await _unitWork.Repository<Order, Guid>().GetByIdWithSpecAsync(specs);

            if (order is null)
                throw new Exception($"There is no order with id {id}");

            var mappedOrder = _mapper.Map<OrderDto>(order);

            return mappedOrder;
        }
        public async Task<OrderStatus?> UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            var order = await _unitWork.Repository<Order, Guid>().GetByIdAsync(orderId);

            if (order == null) return null;

            order.Status = status;

            _unitWork.Repository<Order, Guid>().Update(order);
            await _unitWork.CompleteAsync();

            return order.Status;
        }

    }
}
