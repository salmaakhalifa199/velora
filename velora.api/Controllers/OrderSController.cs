using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using velora.core.Entities.OrderEntities;
using velora.services.HandlerResponses;
using velora.services.Services.OrderService;
using velora.services.Services.OrderService.Dto;

namespace velora.api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderSController : APIBaseController
    {
        private readonly IOrderService _orderService;

        public OrderSController(IOrderService orderService )
        {
            _orderService = orderService;
        }

        [HttpPost]

        public async Task<ActionResult<OrderDto>> CreateOrderAsync([FromBody] CreateOrderDto orderDto)
        {
            var order = await _orderService.CreateOrderAsync(orderDto);

            if (order is null)
                return BadRequest(new ApiResponse<string>(null, false, 400, "Error while creating your order"));
            return Ok(order);
        }

        [HttpPost("myorders")]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetAllOrdersForUserAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var orders = await _orderService.GetAllOrdersForUserAsync(email);

            return Ok(orders);

        }

        [HttpPost("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderByIdAsync(Guid id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var order = await _orderService.GetOrderByIdAsync(id , email);

            return Ok(order);

        }

        [HttpPut("{orderId}/status")]
        public async Task<ActionResult> UpdateOrderStatus(Guid orderId, [FromBody] OrderStatus status)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, status);
            if (!result) return BadRequest("Failed to update order status.");
            return NoContent();
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethods>>> GetAllDeliveryMethodsAsync()
         => Ok(await _orderService.GetAllDeliveryMethodsAsync());
    }
}
